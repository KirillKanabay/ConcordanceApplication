using System;

namespace Concordance.FSM
{
    public class StateTransition
    {
        public readonly ParseState CurrentState;
        public readonly ParseEvent Event;
        public readonly Action Action;

        public StateTransition(ParseState currentState, ParseEvent parseEvent)
        {
            CurrentState = currentState;
            Event = parseEvent;
        }
        
        public StateTransition(ParseState currentState, ParseEvent parseEvent, Action action)
        {
            CurrentState = currentState;
            Event = parseEvent;
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