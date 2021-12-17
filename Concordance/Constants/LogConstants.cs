namespace Concordance.Constants
{
    public static class LogConstants
    {
        public const string Info
            = "INFO:";

        public const string Error
            = "ERROR:";

        public const string Warning
            = "WARNING:";

        public const string LogInfoIsEmpty
            = "Logger information message can't be whitespace or null!";

        public const string LogErrorIsEmpty
            = "Logger error message can't be whitespace or null!";

        public const string LogWarningIsEmpty
            = "Logger warning message can't be whitespace or null!";

        public const string ConcordanceReportForWritingIsNull
            = "The process cannot write report, because concordance report is null!";

        public const string TextForReportingIsNull
            = "The process cannot create report, because text is null!";

        public const string TextPathNotExists
            = "Text path not exists!";

        public const string PageSizeLessThanZero
            = "Page size must be more 0!";

        public const string TextOptionsSectionNotExists
            = "Cannot find \"TextOptions\" section in config file!";

        public const string TextOptionsForParsingTextIsNull
            = "The process cannot parse text, because text options is null!";

        public const string FileNotExistsOrUsedByAnotherProcess
            = "File not exists or being used by another process!";

        public const string ParserError
            = "Parser error:";

        public const string StartWritingReportToConsole
            = "Start writing concordance report to console.";

        public const string StartWritingReportToFile
            = "Start writing concordance report to file:";

        public const string StartCreatingReport
            = "Start creating concordance report.";

        public const string StartParsingText
            = "Start parsing text.";

        public const string StartCreatingDirectory
            = "Start creating directory:";

        public const string WroteReportToConsole
            = "Successfully wrote concordance report to console.";

        public const string WroteReportToFile
            = "Successfully wrote concordance report to file:";

        public const string CreatedReport
            = "Successfully created concordance report.";

        public const string ParsedText
            = "Successfully parsed text.";
    }
}
