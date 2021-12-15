using System.Linq;

namespace Concordance.FSM.States
{
    public class StateGenerator : IStateGenerator
    {
        private readonly char[] _endSentenceSeparators = {'.', '!', '?'};

        public State Generate(char ch)
        {
            State parseState;

            if (_endSentenceSeparators.Contains(ch))
            {
                parseState = State.EndSentenceSeparator;
            }
            else if(ch== '\n' || ch =='\r')
            {
                parseState = State.NewLine;
            }
            else if (char.IsLetterOrDigit(ch) || ch == '\'')
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
