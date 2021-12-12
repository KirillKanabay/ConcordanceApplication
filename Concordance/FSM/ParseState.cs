namespace Concordance.FSM
{
    public enum ParseState
    {
        Inactive,
        Letter,
        Separator,
        EndSentenceSeparator,
        NewLine,
        EndOfFile,
    }
}