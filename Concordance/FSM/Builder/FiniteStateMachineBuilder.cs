using System;
using System.Collections.Generic;
using Concordance.FSM.Events;
using Concordance.FSM.States;

namespace Concordance.FSM.Builder
{
    public class FiniteStateMachineBuilder : 
        IFiniteStateMachineBuilder, 
        IToSetterBuilder,
        IByEventSetterBuilder,
        IActionSetterBuilder
    {
        private readonly IDictionary<StateTransition, State> _transitions;

        private State _from;
        private State _to;
        private Event _byEvent;
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

        public IByEventSetterBuilder To(State state)
        {
            _to = state;

            return this;
        }

        public IActionSetterBuilder ByEvent(Event parseEvent)
        {
            _byEvent = parseEvent;

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
            StateTransition transition = new StateTransition(_from, _byEvent, _action);

            if (!_transitions.ContainsKey(transition))
            {
                _transitions.Add(transition, _to);
            }
        }
    }
}
