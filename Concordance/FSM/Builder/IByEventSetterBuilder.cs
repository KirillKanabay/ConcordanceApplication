using Concordance.FSM.Events;

namespace Concordance.FSM.Builder
{
    public interface IByEventSetterBuilder
    {
        IActionSetterBuilder ByEvent(Event parseEvent);
    }
}
