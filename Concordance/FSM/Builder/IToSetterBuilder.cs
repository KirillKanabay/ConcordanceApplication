using Concordance.FSM.States;

namespace Concordance.FSM.Builder
{
    public interface IToSetterBuilder
    {
        IByEventSetterBuilder To(State state);
    }
}
