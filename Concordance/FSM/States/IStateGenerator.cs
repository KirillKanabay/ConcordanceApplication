namespace Concordance.FSM.States
{
    public interface IStateGenerator
    {
        State Generate(char ch);
    }
}