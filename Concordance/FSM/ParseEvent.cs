namespace Concordance.FSM
{
    public enum ParseEvent
    {
        ReadLetter,
        ReadSeparator,
        ReadEndSentenceSeparator,
        ReadNewLine,
        EndOfFile
    }
}