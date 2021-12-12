using Concordance.FSM.Events;
using Concordance.FSM.States;

namespace Concordance.FSM
{
    public interface IFiniteStateMachine
    {
        State CurrentState { get; }
        void MoveNext(Event parseEvent);
    }
}
