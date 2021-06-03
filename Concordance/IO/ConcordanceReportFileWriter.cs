using System.IO;
using Concordance.Interfaces;

namespace Concordance.IO
{
    public class ConcordanceReportFileWriter:IConcordanceReportWriter
    {
        private readonly string _directory;
        public ConcordanceReportFileWriter(string outputDirectory)
        {
            _directory = outputDirectory;
        }
        public void Write(IConcordanceReport reportService)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            File.WriteAllText($"{_directory}/{reportService.Text.Name}_ConcordanceReport.txt", reportService.ToString());
        }
    }
}
