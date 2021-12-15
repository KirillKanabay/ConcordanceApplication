using Concordance.FSM.States;

namespace Concordance.FSM.Builder
{
    public interface IToSetterBuilder
    {
        IActionSetterBuilder To(State state);
    }
}
