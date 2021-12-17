namespace Concordance.Constants
{
    public static class DataConstants
    {
        public const char EmptyChar 
            = ' ';
        
        public static readonly char[] EndSentenceSeparators 
            = { '.', '!', '?' };

        public const char NewLine 
            = '\n';

        public const char CarriageReturn
            = '\r';

        public const char SingleQuote
            = '\'';

        public const char Whitespace
            = ' ';

        public const int MinPageSize
            = 1;

        public const string ConfigurationPath
            = "appsettings.json";

        public const string ConfigurationTextOptionsSection
            = "TextOptions";

        public const string ConfigurationOutputDirSection
            = "OutputDirectory";

        public const string DefaultDir
            = "concordance_reports";

        public const string ReportFileName
            = "ConcordanceReport.txt";
    }
}
