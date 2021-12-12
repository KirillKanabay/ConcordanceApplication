using System;
using System.Collections.Generic;
using System.Linq;

namespace Concordance.FSM
{
    public class FiniteParseStateMachine
    {
        private readonly IDictionary<StateTransition, ParseState> _transitions;
        public ParseState CurrentState { get; private set; }

        public FiniteParseStateMachine(IDictionary<StateTransition, ParseState> transitions)
        {
            CurrentState = ParseState.Inactive;
            _transitions = transitions;
        }

        public void MoveNext(ParseEvent parseEvent)
        {
            StateTransition transition = new StateTransition(CurrentState, parseEvent);
            ParseState nextState;
            if (!_transitions.TryGetValue(transition, out nextState))
            {
                throw new Exception("Invalid transition");
            }

            var action = _transitions.Keys.FirstOrDefault(k => k.Equals(transition))?.Action;
            action?.Invoke();
            
            CurrentState = nextState;
        }
    }
}