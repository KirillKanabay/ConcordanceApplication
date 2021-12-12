namespace Concordance.FSM
{
    public enum ParseState
    {
        Inactive,
        Letter,
        Separator,
        NewLine,
        EndOfFile,
    }
}