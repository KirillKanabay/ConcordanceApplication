using System.Linq;

namespace Concordance.FSM.Events
{
    public class EventGenerator : IEventGenerator
    {
        private readonly char[] _endSentenceSeparators = {'.', '!', '?'};

        public Event Generate(char ch)
        {
            Event parseEvent;

            if (_endSentenceSeparators.Contains(ch))
            {
                parseEvent = Event.ReadEndSentenceSeparator;
            }
            else if(ch== '\n' || ch =='\r')
            {
                parseEvent = Event.ReadNewLine;
            }
            else if (char.IsLetterOrDigit(ch) || ch == '\'')
            {
                parseEvent = Event.ReadLetter;
            }
            else
            {
                parseEvent = Event.ReadSeparator;
            }

            return parseEvent;
        }
    }
}
