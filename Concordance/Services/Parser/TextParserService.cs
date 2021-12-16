using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Concordance.Constants;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.FSM.States.Parser;
using Concordance.Helpers.Logger;
using Concordance.Model;
using Concordance.Model.Options;
using Concordance.Model.TextElements;
using FluentValidation;
using FluentValidation.Results;

namespace Concordance.Services.Parser
{
    public class TextParserService : ITextParserService
    {
        private readonly IStateParser _stateGenerator;
        private readonly IFiniteStateMachineBuilder _fsmBuilder;
        private readonly IFiniteStateMachine _fsm;
        private readonly IValidator<TextOptions> _textOptionsValidator;
        private readonly ILogger _logger;

        private readonly IList<Page> _pagesBuffer;
        private readonly IList<Sentence> _sentenceBuffer;
        private readonly IList<BaseSentenceElement> _sentenceElementsBuffer;
        private readonly IList<char> _charBuffer;

        private TextOptions _options;

        private char _lastReadChar;
        private int _lineCount = 1;

        public TextParserService(IStateParser stateGenerator, 
            IFiniteStateMachineBuilder fsmBuilder,
            IValidator<TextOptions> textOptionsValidator,
            ILogger logger)
        {
            _logger = logger;
            _stateGenerator = stateGenerator;
            _fsmBuilder = fsmBuilder;
            _textOptionsValidator = textOptionsValidator;
            _fsm = InitFSM();

            _pagesBuffer = new List<Page>();
            _sentenceBuffer = new List<Sentence>();
            _sentenceElementsBuffer = new List<BaseSentenceElement>();
            _charBuffer = new List<char>();
            
        }

        public ServiceResult<Text> Parse(TextOptions options)
        {
            _logger.Information(InfoLogConstants.StartParsingText);

            if (options == null)
            {
                _logger.Error(ErrorLogConstants.TextOptionsForParsingTextIsNull);

                return new ServiceResult<Text>()
                {
                    Error = ErrorLogConstants.TextOptionsForParsingTextIsNull,
                    IsSuccess = false,
                };
            }

            ValidationResult validationResult = _textOptionsValidator.Validate(options);

            if (!validationResult.IsValid)
            {

                foreach (var error in validationResult.Errors)
                {
                    _logger.Error(error.ErrorMessage);
                }

                return new ServiceResult<Text>()
                {
                    Error = validationResult.ToString("\n"),
                    IsSuccess = false,
                };
            }

            _options = options;

            try
            {
                using (var sr = new StreamReader(options.Path))
                {
                    char[] buffer = new char[4096];
                    while (sr.Read(buffer, 0, buffer.Length) != 0)
                    {
                        foreach (var c in buffer)
                        {
                            State nextState = _stateGenerator.Parse(c);
                            _lastReadChar = c;
                            _fsm.MoveNext(nextState);
                        }
                    }

                    _fsm.MoveNext(State.EndOfFile);
                }
            }
            catch (IOException)
            {
                _logger.Error($"{ErrorLogConstants.FileNotExistsOrUsedByAnotherProcess} {options.Path}");
            }
            

            _logger.Success(SuccessLogConstants.ParsedText);

            var result = new ServiceResult<Text>()
            {
                IsSuccess = true,
                Data = new Text()
                {
                    Name = options.Name,
                    Pages = new List<Page>(_pagesBuffer)
                }
            };

            Clear();

            return result;
        }

        private IFiniteStateMachine InitFSM()
        {
            _fsmBuilder.From(State.Inactive).To(State.Letter).Action(AppendToCharBuffer);

            _fsmBuilder.From(State.Letter).To(State.Letter).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Letter).To(State.Separator).Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.NewLine).Action(AppendNewLineAndWord);
            _fsmBuilder.From(State.Letter).To(State.EndSentenceSeparator).Action(AppendWord);

            _fsmBuilder.From(State.Separator).To(State.Separator).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Separator).To(State.Letter).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.NewLine).Action(AppendNewLine);
            _fsmBuilder.From(State.Separator).To(State.EndSentenceSeparator).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.EndOfFile).Action(AppendPage);

            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndSentenceSeparator).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Letter).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Separator).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.NewLine).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndOfFile).Action(AppendPage);

            _fsmBuilder.From(State.NewLine).To(State.Letter).Action(AppendSeparator);
            _fsmBuilder.From(State.NewLine).To(State.NewLine).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.NewLine).To(State.Separator).Action(AppendSeparator);

            return _fsmBuilder.Build();
        }

        private void AppendNewLine()
        {
            _lineCount++;
            
            if (_lineCount % _options.PageSize == 0)
            {
                AppendPage();
            }
        }

        private void AppendNewLineAndWord()
        {
            AppendWord();
            AppendNewLine();
        }

        private void AppendToCharBuffer()
        {
            _charBuffer.Add(_lastReadChar);
        }

        private void AppendWord()
        {
            string word = new string(_charBuffer.ToArray());
            Word wordInstance = new Word {Content = word};
            _sentenceElementsBuffer.Add(wordInstance);

            _charBuffer.Clear();

            AppendToCharBuffer();
        }

        private void AppendSeparator()
        {
            string separator = new string(_charBuffer.ToArray());
            Separator separatorInstance = new Separator() {Content = separator};
            _sentenceElementsBuffer.Add(separatorInstance);

            _charBuffer.Clear();
            AppendToCharBuffer();
        }

        private void AppendSentence()
        {
            AppendSeparator();

            Sentence sentence = new Sentence()
            {
                SentenceElements = new List<BaseSentenceElement>(_sentenceElementsBuffer)
            };
            _sentenceBuffer.Add(sentence);

            _sentenceElementsBuffer.Clear();
        }

        private void AppendPage()
        {
            AppendSentence();

            Page page = new Page()
            {
                Sentences = new List<Sentence>(_sentenceBuffer),
                Number = (int) Math.Ceiling(_lineCount / (double) _options.PageSize),
            };

            _pagesBuffer.Add(page);

            _sentenceBuffer.Clear();
            _sentenceElementsBuffer.Clear();
            _charBuffer.Clear();
        }

        private void Clear()
        {
            _pagesBuffer.Clear();
            _sentenceBuffer.Clear();
            _sentenceElementsBuffer.Clear();
            _charBuffer.Clear();

            _options = null;

            _lineCount = 1;
        }
    }
}