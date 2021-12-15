using System;

namespace Concordance.FSM.States
{
    public class StateTransition
    {
        public readonly State CurrentState;
        public readonly State NextState;
        public readonly Action Action;

        public StateTransition(State currentState, State nextState)
        {
            CurrentState = currentState;
            NextState = nextState;
        }
        
        public StateTransition(State currentState, State nextState, Action action) : this(currentState, nextState)
        {
            Action = action;
        }
        
        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 17 * NextState.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is StateTransition other 
                   && CurrentState == other.CurrentState 
                   && NextState == other.NextState;
        }
    }
}