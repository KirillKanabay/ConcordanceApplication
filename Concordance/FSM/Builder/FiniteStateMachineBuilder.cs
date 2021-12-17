using System;
using System.Collections.Generic;
using Concordance.FSM.States;

namespace Concordance.FSM.Builder
{
    public class FiniteStateMachineBuilder : 
        IFiniteStateMachineBuilder, 
        IToSetterBuilder,
        IActionSetterBuilder
    {
        private readonly IDictionary<StateTransition, State> _transitions;

        private State _from;
        private State _to;
        private Action _action;

        public FiniteStateMachineBuilder()
        {
            _transitions = new Dictionary<StateTransition, State>();
        }
        
        public IFiniteStateMachine Build()
        {
            return new FiniteStateMachine(_transitions);
        }

        public IToSetterBuilder From(State state)
        {
            _from = state;

            return this;
        }

        public IActionSetterBuilder To(State state)
        {
            _to = state;

            return this;
        }
        
        public IFiniteStateMachineBuilder Action(Action action)
        {
            _action = action;
            
            AppendTransition();

            return this;
        }

        private void AppendTransition()
        {
            StateTransition transition = new StateTransition(_from, _to, _action);

            if (!_transitions.ContainsKey(transition))
            {
                _transitions.Add(transition, _to);
            }
        }
    }
}
