using Concordance.FSM.States;

namespace Concordance.FSM.Builder
{
    public interface IFromSetterBuilder
    {
        IToSetterBuilder From(State state);
    }
}
