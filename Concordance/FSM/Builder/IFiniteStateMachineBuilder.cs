namespace Concordance.FSM.Builder
{
    public interface IFiniteStateMachineBuilder : 
        IFromSetterBuilder
    {
        IFiniteStateMachine Build();
    }
}
