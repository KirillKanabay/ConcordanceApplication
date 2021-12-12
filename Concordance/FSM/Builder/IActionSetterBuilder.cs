using System;

namespace Concordance.FSM.Builder
{
    public interface IActionSetterBuilder
    {
        IFiniteStateMachineBuilder Action(Action action);
    }
}
