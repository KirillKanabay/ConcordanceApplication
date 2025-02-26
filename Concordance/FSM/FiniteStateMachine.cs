﻿using System;
using System.Collections.Generic;
using System.Linq;
using Concordance.FSM.States;

namespace Concordance.FSM
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        private readonly IDictionary<StateTransition, State> _transitions;
        public State CurrentState { get; private set; }

        public FiniteStateMachine(IDictionary<StateTransition, State> transitions)
        {
            CurrentState = State.Inactive;
            _transitions = transitions;
        }

        public void MoveNext(State state)
        {
            StateTransition transition = new StateTransition(CurrentState, state);
            if (!_transitions.TryGetValue(transition, out var nextState))
            {
                throw new Exception($"Invalid transition: {CurrentState} -> {state}");
            }
            var action = _transitions.Keys.FirstOrDefault(k => k.Equals(transition))?.Action;
            action?.Invoke();
            
            CurrentState = nextState;
        }
    }
}