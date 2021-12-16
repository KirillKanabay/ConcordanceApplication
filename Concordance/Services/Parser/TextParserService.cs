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
        private IList<char> _charBuffer;

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
                    Error = validationResult.ToString(CharConstants.NewLine.ToString()),
                    IsSuccess = false,
                };
            }

            _options = options;

            try
            {
                using (var sr = new StreamReader(options.Path))
                {
                    int lastReadValue;
                    while ((lastReadValue = sr.Read()) != -1)
                    {
                        _lastReadChar = (char) lastReadValue;
                        State nextState = _stateGenerator.Parse(_lastReadChar);
                        _charBuffer.Add(_lastReadChar);

                        try
                        {
                            _fsm.MoveNext(nextState);
                        }
                        catch (Exception e)
                        {
                            _logger.Error($"{ErrorLogConstants.ParserError} {e.Message}");
                            
                            sr.Dispose();

                            return new ServiceResult<Text>()
                            {
                                IsSuccess = false,
                                Error = $"{ErrorLogConstants.ParserError} {e.Message}",
                            };
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
            _fsmBuilder.From(State.Inactive).To(State.Letter).Action(() => { });

            _fsmBuilder.From(State.Letter).To(State.Letter).Action(() => { });
            _fsmBuilder.From(State.Letter).To(State.Separator).Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.Whitespace).Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.NewLine).Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.EndSentenceSeparator).Action(AppendWord);

            _fsmBuilder.From(State.Whitespace).To(State.Letter).Action(AppendSeparator);
            _fsmBuilder.From(State.Whitespace).To(State.NewLine).Action(ClearCharBuffer);
            _fsmBuilder.From(State.Whitespace).To(State.Separator).Action(ClearCharBuffer);
            _fsmBuilder.From(State.Whitespace).To(State.EndOfFile).Action(AppendPage);

            _fsmBuilder.From(State.Separator).To(State.Separator).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.Whitespace).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.Letter).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.NewLine).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.EndSentenceSeparator).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.EndOfFile).Action(AppendPage);

            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndSentenceSeparator).Action(() => { });
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Letter).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Separator).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.NewLine).Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndOfFile).Action(AppendPage);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Whitespace).Action(AppendSentence);

            _fsmBuilder.From(State.NewLine).To(State.Letter).Action(AppendNewLine);
            _fsmBuilder.From(State.NewLine).To(State.NewLine).Action(() => { });
            _fsmBuilder.From(State.NewLine).To(State.Separator).Action(AppendNewLine);

            return _fsmBuilder.Build();
        }

        private void ClearCharBuffer()
        {
            _charBuffer = _charBuffer.TakeLast(1).ToList();
        }

        private void AppendNewLine()
        {
            AppendSeparator();
            if (_lineCount % _options.PageSize == 0)
            {
                AppendPage();
            }

            _lineCount++;
        }
        
        private void AppendWord()
        {
            string word = new string(_charBuffer.SkipLast(1).ToArray());
            Word wordInstance = new Word {Content = word};
            _sentenceElementsBuffer.Add(wordInstance);

            ClearCharBuffer();
        }

        private void AppendSeparator()
        {
            string separator = new string(_charBuffer.SkipLast(1).ToArray());
            Separator separatorInstance = new Separator() {Content = separator};
            _sentenceElementsBuffer.Add(separatorInstance);

            ClearCharBuffer();
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