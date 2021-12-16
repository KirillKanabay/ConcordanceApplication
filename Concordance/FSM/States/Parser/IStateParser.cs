namespace Concordance.FSM.States.Parser
{
    public interface IStateParser
    {
        State Parse(char ch);
    }
}