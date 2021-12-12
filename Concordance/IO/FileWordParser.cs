﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.Events;
using Concordance.FSM.States;
using Concordance.Interfaces;
using Concordance.Model;

namespace Concordance.IO
{
    public class FileWordParser : IWordParser
    {
        private readonly string _path;
        private readonly int _pageSize;

        private IEventGenerator _eventGenerator;
        private IFiniteStateMachineBuilder _fsmBuilder;

        private IList<Page> _pagesBuffer;
        private IList<Sentence> _sentenceBuffer;
        private IList<BaseSentenceElement> _sentenceElementsBuffer;
        private IList<char> _charBuffer;

        private char _lastReadChar;
        private int _lineCount;
        private IFiniteStateMachine _fsm;
        
        public FileWordParser(string path, int pageSize)
        {
            _path = path;
            _pageSize = pageSize;

            _pagesBuffer = new List<Page>();
            _sentenceBuffer = new List<Sentence>();
            _sentenceElementsBuffer= new List<BaseSentenceElement>();
            _charBuffer = new List<char>();

            _eventGenerator = new EventGenerator();

            _fsmBuilder = new FiniteStateMachineBuilder();

            InitFSM();
        }

        private void InitFSM()
        {
           

            _fsmBuilder.From(State.Inactive).To(State.Letter).ByEvent(Event.ReadLetter)
                .Action(AppendToCharBuffer);

            _fsmBuilder.From(State.Letter)

            _fsm = new FiniteParseStateMachine(transitions);
        }

        private void IncLineCount()
        {
            _lineCount++;

            if (_lineCount == _pageSize)
            {
                AppendPage();
            }

            _lineCount = 0;
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
            Separator separatorInstance = new Separator() { Content = separator };
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



        public void AppendPage()
        {
            AppendSentence();

            Page page = new Page()
            {
                Sentences = new List<Sentence>(_sentenceBuffer)
            };

            _pagesBuffer.Add(page);

            _sentenceBuffer.Clear();
            _sentenceElementsBuffer.Clear();
            _charBuffer.Clear();
        }

        public async Task<ParserResult> Parse()
        {
            //todo: Fluent validation
            if (_pageSize < 0)
            {
                return new ParserResult()
                {
                    Error = "Page size can't be less than zero",
                    IsSuccess = false,
                };
            }

            if (!File.Exists(_path))
            {
                return new ParserResult()
                {
                    Error = "File doesn't exist",
                    IsSuccess = false,
                };
            }

            await using (var fs = new FileStream(_path, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    int nextCh;

                    while ((nextCh = sr.Read()) != -1)
                    {
                        Event parseEvent = _eventGenerator.Generate((char)nextCh);
                        _lastReadChar = (char) nextCh;
                        _fsm.MoveNext(parseEvent);
                    }

                    _fsm.MoveNext(Event.EndOfFile);
                }
            }

            return new ParserResult()
            {
                IsSuccess = true,
                Text = new Text() {Name = "test", Pages = _pagesBuffer}
            };
        }
    }
}