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
                {new StateTransition(ParseState.Letter, ParseEvent.ReadSeparator, AppendWord), ParseState.Separator},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                {new StateTransition(ParseState.Letter, ParseEvent.ReadEndSentenceSeparator, AppendWord), ParseState.EndSentenceSeparator},

                {new StateTransition(ParseState.Separator, ParseEvent.ReadSeparator, AppendToCharBuffer), ParseState.Separator},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadLetter, AppendSeparator), ParseState.Letter},
                {new StateTransition(ParseState.Separator, ParseEvent.ReadNewLine, IncLineCount), ParseState.NewLine},
                
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadEndSentenceSeparator, AppendToCharBuffer), ParseState.EndSentenceSeparator},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadLetter, AppendSentence), ParseState.Letter},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadSeparator, AppendSentence), ParseState.Separator},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.ReadNewLine, AppendSentence), ParseState.NewLine},
                {new StateTransition(ParseState.EndSentenceSeparator, ParseEvent.EndOfFile, AppendPage), ParseState.EndOfFile},
                
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadLetter, AppendSeparator), ParseState.Letter},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadNewLine, AppendToCharBuffer), ParseState.NewLine},
                {new StateTransition(ParseState.NewLine, ParseEvent.ReadSeparator, AppendSeparator), ParseState.Separator}
            };

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
                        ParseEvent parseEvent = _eventGenerator.Generate((char)nextCh);
                        _lastReadChar = (char) nextCh;
                        _fsm.MoveNext(parseEvent);
                    }

                    _fsm.MoveNext(ParseEvent.EndOfFile);
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