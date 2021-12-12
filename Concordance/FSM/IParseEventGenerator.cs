namespace Concordance.FSM
{
    public interface IParseEventGenerator
    {
        ParseEvent Generate(char ch);
    }
}