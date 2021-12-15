using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.Model;

namespace Concordance.Parser
{
    public class TextParser : ITextParser, IDisposable
    {
        private readonly IStateGenerator _eventGenerator;
        private readonly IFiniteStateMachineBuilder _fsmBuilder;
        private IFiniteStateMachine _fsm;

        private readonly IList<Page> _pagesBuffer;
        private readonly IList<Sentence> _sentenceBuffer;
        private readonly IList<BaseSentenceElement> _sentenceElementsBuffer;
        private readonly IList<char> _charBuffer;

        private char _lastReadChar;
        private int _lineCount = 1;

        private readonly Stream _stream;
        private readonly int _pageSize;

        public TextParser(Stream stream, int pageSize)
        {
            _stream = stream;
            _pageSize = pageSize;

            _pagesBuffer = new List<Page>();
            _sentenceBuffer = new List<Sentence>();
            _sentenceElementsBuffer = new List<BaseSentenceElement>();
            _charBuffer = new List<char>();

            _eventGenerator = new StateGenerator();

            _fsmBuilder = new FiniteStateMachineBuilder();

            InitFSM();
        }

        public async Task<ParserResult> Parse()
        {
            if (_pageSize < 0)
            {
                return new ParserResult()
                {
                    Error = "Page size can't be less than zero",
                    IsSuccess = false,
                };
            }

            if (_stream == null)
            {
                return new ParserResult()
                {
                    Error = "Stream can't be null",
                    IsSuccess = false,
                };
            }

            using (var sr = new StreamReader(_stream))
            {
                char[] buffer = new char[4096];
                while (await sr.ReadAsync(buffer, 0, buffer.Length) != 0)
                {
                    foreach (var c in buffer)
                    {
                        State nextState = _eventGenerator.Generate(c);
                        _lastReadChar = c;
                        _fsm.MoveNext(nextState);
                    }
                }

                _fsm.MoveNext(State.EndOfFile);
            }

            return new ParserResult()
            {
                IsSuccess = true,
                Text = new Text() { Name = "test", Pages = _pagesBuffer }
            };
        }


        private void InitFSM()
        {
            _fsmBuilder.From(State.Inactive).To(State.Letter).Action(AppendToCharBuffer);

            _fsmBuilder.From(State.Letter).To(State.Letter).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Letter).To(State.Separator).Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.NewLine).Action(IncLineCount);
            _fsmBuilder.From(State.Letter).To(State.EndSentenceSeparator).Action(AppendWord);

            _fsmBuilder.From(State.Separator).To(State.Separator).Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Separator).To(State.Letter).Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.NewLine).Action(IncLineCount);
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

            _fsm = _fsmBuilder.Build();
        }

        private void IncLineCount()
        {
            _lineCount++;

            if (_lineCount == _pageSize)
            {
                AppendPage();
            }
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
                Number = (int)Math.Ceiling(_lineCount / (double)_pageSize),
            };

            _pagesBuffer.Add(page);

            _sentenceBuffer.Clear();
            _sentenceElementsBuffer.Clear();
            _charBuffer.Clear();
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}