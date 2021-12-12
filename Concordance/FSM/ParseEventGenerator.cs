using System.Linq;

namespace Concordance.FSM
{
    public class ParseEventGenerator : IParseEventGenerator
    {
        private readonly char[] _endSentenceSeparators = {'.', '!', '?'};

        public ParseEvent Generate(int ch)
        {
            if (ch == -1)
            {
                return ParseEvent.EndOfFile;
            }

            ParseEvent parseEvent;
            char character = (char) ch;

            if (_endSentenceSeparators.Contains(character))
            {
                parseEvent = ParseEvent.ReadEndSentenceSeparator;
            }
            else if(character == '\n' || character =='\r')
            {
                parseEvent = ParseEvent.ReadNewLine;
            }
            else if (char.IsLetterOrDigit(character))
            {
                parseEvent = ParseEvent.ReadLetter;
            }
            else
            {
                parseEvent = ParseEvent.ReadSeparator;
            }

            return parseEvent;
        }
    }
}
