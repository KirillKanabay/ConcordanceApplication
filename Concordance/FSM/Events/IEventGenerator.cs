namespace Concordance.FSM.Events
{
    public interface IEventGenerator
    {
        Event Generate(char ch);
    }
}