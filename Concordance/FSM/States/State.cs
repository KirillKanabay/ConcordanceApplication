namespace Concordance.FSM.States
{
    public enum State
    {
        Inactive,
        Letter,
        Whitespace,
        Separator,
        EndSentenceSeparator,
        NewLine,
        EndOfFile,
    }
}