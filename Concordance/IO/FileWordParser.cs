using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Concordance.FSM;
using Concordance.Interfaces;
using Concordance.Model;

namespace Concordance.IO
{
    public class FileWordParser : IWordParser
    {
        private readonly string _path;
        private readonly int _pageSize;

        private IParseEventGenerator _eventGenerator;

        private IList<Page> _pagesBuffer;
        private IList<Sentence> _sentenceBuffer;
        private IList<BaseSentenceElement> _sentenceElementsBuffer;
        private IList<char> _charBuffer;

        private char _lastReadChar;
        private int _lineCount;
        private FiniteParseStateMachine _fsm;
        
        public FileWordParser(string path, int pageSize)
        {
            _path = path;
            _pageSize = pageSize;

            _pagesBuffer = new List<Page>();
            _sentenceBuffer = new List<Sentence>();
            _sentenceElementsBuffer= new List<BaseSentenceElement>();
            _charBuffer = new List<char>();

            _eventGenerator = new ParseEventGenerator();

            InitFSM();
        }

        private void InitFSM()
        {
            Dictionary<StateTransition, ParseState> transitions = new Dictionary<StateTransition, ParseState>()
            {
                {new StateTransition(ParseState.Inactive, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                
                {new StateTransition(ParseState.Letter, ParseEvent.ReadLetter, AppendToCharBuffer), ParseState.Letter},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadSeparator, AppendWordToSentence), ParseState.Separator},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadEndSentenceSeparator, AppendWordToSentence), ParseState.EndSentenceSeparator},

                {new StateTransition(ParseState.Separator, ParseEvent.ReadSeparator, AppendToCharBuffer), ParseState.Separator},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadLetter, AppendSeparatorToSentence), ParseState.Letter},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadEndSentenceSeparator, AppendToCharBuffer), ParseState.EndSentenceSeparator},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadLetter, AppendToSentenceBuffer), ParseState.Letter},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadSeparator, AppendToSentenceBuffer), ParseState.Separator},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadNewLine, AppendToSentenceBuffer), ParseState.NewLine},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.EndOfFile, AppendToPagesBuffer), ParseState.EndOfFile},
                
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadLetter, AppendSeparatorToSentence), ParseState.Letter},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadNewLine, AppendToCharBuffer), ParseState.NewLine},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadSeparator, AppendSeparatorToSentence), ParseState.Separator}
            };

            _fsm = new FiniteParseStateMachine(transitions);
        }

        private void IncLineCount()
        {
            _lineCount++;

            if (_lineCount == _pageSize)
            {
                AppendToPagesBuffer();
            }
        }

        private void AppendToCharBuffer()
        {
            _charBuffer.Add(_lastReadChar);
        }
        
        private void AppendWordToSentence()
        {
            string word = new string(_charBuffer.ToArray());
            Word wordInstance = new Word {Content = word};
            _sentenceElementsBuffer.Add(wordInstance);
                
            _charBuffer.Clear();
            
            AppendToCharBuffer();
        }

        private void AppendSeparatorToSentence()
        {
            string separator = new string(_charBuffer.ToArray());
            Separator separatorInstance = new Separator() { Content = separator };
            _sentenceElementsBuffer.Add(separatorInstance);

            _charBuffer.Clear();
            AppendToCharBuffer();
        }

        private void AppendToSentenceBuffer()
        {
            Sentence sentence = new Sentence() {SentenceElements = _sentenceElementsBuffer};
            _sentenceBuffer.Add(sentence);

            _sentenceElementsBuffer.Clear();
        }

        public void AppendToPagesBuffer()
        {
            Page page = new Page() {Sentences = _sentenceBuffer};
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
                    while (!sr.EndOfStream)
                    {
                        int nextCh = sr.Read();
                        ParseEvent parseEvent = _eventGenerator.Generate(nextCh);
                        
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
                Text = new Text() {Name = "test", Pages = _pagesBuffer}
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