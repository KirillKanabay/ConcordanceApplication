using System.IO;
using Concordance.Interfaces;

namespace Concordance.Concordance
{
    public class ConcordanceReportFileWriter:IConcordanceReportWriter
    {
        private readonly string _directory;
        public ConcordanceReportFileWriter(string outputDirectory)
        {
            _directory = outputDirectory;
        }

        public void Write(IConcordanceReport report)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            using (var fs = new FileStream($"{_directory}/{report.Text.Name}_ConcordanceReport.txt",
                       FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(fs))
                {
                    char prevFirstChar = ' ';

                    foreach (var item in report.ReportList.Values)
                    {
                        if (char.ToUpper(item.Word.FirstChar) != prevFirstChar)
                        {
                            if (prevFirstChar != ' ')
                            {
                                writer.WriteLine();
                            }

                            prevFirstChar = char.ToUpper(item.Word.FirstChar);
                            writer.WriteLine(prevFirstChar.ToString());
                        }

                        writer.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}
