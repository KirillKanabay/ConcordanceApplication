using System.Linq;
using Concordance.Constants;

namespace Concordance.FSM.States.Parser
{
    public class StateParser : IStateParser
    {
        public State Parse(char ch)
        {
            State parseState;

            if (DataConstants.EndSentenceSeparators.Contains(ch))
            {
                parseState = State.EndSentenceSeparator;
            }
            else if (ch == DataConstants.Whitespace)
            {
                parseState = State.Whitespace;
            }
            else if(ch == DataConstants.NewLine || ch == DataConstants.CarriageReturn)
            {
                parseState = State.NewLine;
            }
            else if (char.IsLetterOrDigit(ch) || ch == DataConstants.SingleQuote)
            {
                parseState = State.Letter;
            }
            else
            {
                parseState = State.Separator;
            }

            return parseState;
        }
    }
}
