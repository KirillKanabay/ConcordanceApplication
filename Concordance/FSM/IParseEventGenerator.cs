namespace Concordance.FSM
{
    public interface IParseEventGenerator
    {
        ParseEvent Generate(int ch);
    }
}