using System.Linq;

namespace Concordance.FSM
{
    public class ParseEventGenerator : IParseEventGenerator
    {
        private readonly char[] _endSentenceSeparators = {'.', '!', '?'};

        public ParseEvent Generate(char ch)
        {
            ParseEvent parseEvent;

            if (_endSentenceSeparators.Contains(ch))
            {
                parseEvent = ParseEvent.ReadEndSentenceSeparator;
            }
            else if(ch== '\n' || ch =='\r')
            {
                parseEvent = ParseEvent.ReadNewLine;
            }
            else if (char.IsLetterOrDigit(ch) || ch == '\'')
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
