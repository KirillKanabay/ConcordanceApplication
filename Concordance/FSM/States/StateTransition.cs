using System;
using Concordance.FSM.Events;

namespace Concordance.FSM.States
{
    public class StateTransition
    {
        public readonly State CurrentState;
        public readonly Event Event;
        public readonly Action Action;

        public StateTransition(State currentState, Event parseEvent)
        {
            CurrentState = currentState;
            Event = parseEvent;
        }
        
        public StateTransition(State currentState, Event parseEvent, Action action) : this(currentState, parseEvent)
        {
            Action = action;
        }
        
        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Event.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Event == other.Event;
        }
    }
}