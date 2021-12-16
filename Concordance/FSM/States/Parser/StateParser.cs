using System.Linq;
using Concordance.Constants;

namespace Concordance.FSM.States.Parser
{
    public class StateParser : IStateParser
    {
        public State Parse(char ch)
        {
            State parseState;

            if (CharConstants.EndSentenceSeparators.Contains(ch))
            {
                parseState = State.EndSentenceSeparator;
            }
            else if(ch == CharConstants.NewLine || ch == CharConstants.CarriageReturn)
            {
                parseState = State.NewLine;
            }
            else if (char.IsLetterOrDigit(ch) || ch == CharConstants.SingleQuote)
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
