using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Concordance.Enums;
using Concordance.FSM;
using Concordance.Interfaces;
using Concordance.Model;

namespace Concordance.IO
{
    public class FileWordParser : IWordParser
    {
        private readonly string _path;
        private readonly int _pageSize;

        private IList<Word> _words;
        private IList<char> _charBuffer;
        private char _lastReadChar;
        private int _lineCount;
        private FiniteParseStateMachine _fsm;
        
        public FileWordParser(string path, int pageSize)
        {
            _path = path;
            _pageSize = pageSize;

            _words = new List<Word>();
            _charBuffer = new List<char>();

            InitFSM();
        }

        private void InitFSM()
        {
            Dictionary<StateTransition, ParseState> transitions = new Dictionary<StateTransition, ParseState>()
            {
                {new StateTransition(ParseState.Inactive, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadSeparator, AppendToWordBuffer), ParseState.Separator},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadSeparator, () => { }), ParseState.Separator},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                {new StateTransition(ParseState.Separator, ParseEvent.EndOfFile, () => { }), ParseState.EndOfFile},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadSeparator, () => { }), ParseState.Separator}
            };

            _fsm = new FiniteParseStateMachine(transitions);
        }
        private void AppendToCharBuffer()
        {
            _charBuffer.Add(_lastReadChar);
        }
        private void IncLineCount()
        {
            _lineCount++;
        }
        private void AppendToWordBuffer()
        {
            string word = new string(_charBuffer.ToArray());
            Word wordInstance = new Word(word);
            _words.Add(wordInstance);
                
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
                    while (!sr.EndOfStream)
                    {
                        int nextCh = sr.Read();
                        ParseEvent parseEvent = GetParseEventByChar(nextCh);
                        
                        if(parseEvent != ParseEvent.EndOfFile)
                        { 
                            _lastReadChar = (char)nextCh;
                        }

                        _fsm.MoveNext(parseEvent);
                    }
                }
            }

            return new ParserResult()
            {
                IsSuccess = true,
                Words = _words,
            };
        }

        private ParseEvent GetParseEventByChar(int nextCh)
        {
            if (nextCh == -1)
            {
                return ParseEvent.EndOfFile;
            }

            char ch = (char) nextCh;

            ParseEvent parseEvent;
            if (char.IsLetterOrDigit(ch))
            {
                parseEvent = ParseEvent.ReadLetter;
            }
            else if (ch == '\n')
            {
                parseEvent = ParseEvent.ReadNewLine;
            }
            else
            {
                parseEvent = ParseEvent.ReadSeparator;
            }

            return parseEvent;
        }
    }
}