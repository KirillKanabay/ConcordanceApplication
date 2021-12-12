using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.Events;
using Concordance.FSM.States;
using Concordance.Model;

namespace Concordance.Parser
{
    public class StreamTextParser : ITextParser, IDisposable
    {
        private readonly IEventGenerator _eventGenerator;
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

        public StreamTextParser(Stream stream, int pageSize)
        {
            _stream = stream;
            _pageSize = pageSize;

            _pagesBuffer = new List<Page>();
            _sentenceBuffer = new List<Sentence>();
            _sentenceElementsBuffer = new List<BaseSentenceElement>();
            _charBuffer = new List<char>();

            _eventGenerator = new EventGenerator();

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
                        Event parseEvent = _eventGenerator.Generate(c);
                        _lastReadChar = c;
                        _fsm.MoveNext(parseEvent);
                    }
                }

                _fsm.MoveNext(Event.EndOfFile);
            }

            return new ParserResult()
            {
                IsSuccess = true,
                Text = new Text() { Name = "test", Pages = _pagesBuffer }
            };
        }


        private void InitFSM()
        {
            _fsmBuilder.From(State.Inactive).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendToCharBuffer);

            _fsmBuilder.From(State.Letter).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Letter).To(State.Separator).ByEvent(Event.ReadSeparator)
                .Action(AppendWord);
            _fsmBuilder.From(State.Letter).To(State.NewLine).ByEvent(Event.ReadNewLine)
                .Action(IncLineCount);
            _fsmBuilder.From(State.Letter).To(State.EndSentenceSeparator).ByEvent(Event.ReadEndSentenceSeparator)
                .Action(AppendWord);

            _fsmBuilder.From(State.Separator).To(State.Separator).ByEvent(Event.ReadSeparator)
                .Action(AppendToCharBuffer);
            _fsmBuilder.From(State.Separator).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.NewLine).ByEvent(Event.ReadNewLine)
                .Action(IncLineCount);
            _fsmBuilder.From(State.Separator).To(State.EndSentenceSeparator).ByEvent(Event.ReadEndSentenceSeparator)
                .Action(AppendSeparator);
            _fsmBuilder.From(State.Separator).To(State.EndOfFile).ByEvent(Event.EndOfFile)
                .Action(AppendPage);

            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndSentenceSeparator)
                .ByEvent(Event.ReadEndSentenceSeparator)
                .Action(AppendToCharBuffer);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.Separator).ByEvent(Event.ReadSeparator)
                .Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.NewLine).ByEvent(Event.ReadNewLine)
                .Action(AppendSentence);
            _fsmBuilder.From(State.EndSentenceSeparator).To(State.EndOfFile).ByEvent(Event.EndOfFile)
                .Action(AppendPage);

            _fsmBuilder.From(State.NewLine).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendSeparator);
            _fsmBuilder.From(State.NewLine).To(State.NewLine).ByEvent(Event.ReadNewLine)
                .Action(AppendToCharBuffer);
            _fsmBuilder.From(State.NewLine).To(State.Separator).ByEvent(Event.ReadSeparator)
                .Action(AppendSeparator);

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
                Number = _lineCount / _pageSize,
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