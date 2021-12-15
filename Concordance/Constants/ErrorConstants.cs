namespace Concordance.Constants
{
    public static class ErrorConstants
    {
        public const string LogInfoIsEmpty 
            = "Logger information message can't be whitespace or null!";
        public const string LogErrorIsEmpty 
            = "Logger error message can't be whitespace or null!";
        
        public const string ConcordanceReportForWritingIsNull 
            = "The process cannot write report, because concordance report is null!";

        public const string TextForReportingIsNull 
            = "The process cannot create report, because text is null!";
    }
}
