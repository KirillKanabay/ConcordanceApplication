using System.IO;
using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceFileWriterService:IConcordanceWriter
    {
        private readonly string _directory;
        public ConcordanceFileWriterService(string outputDirectory)
        {
            _directory = outputDirectory;
        }

        public void Write(ConcordanceReport report)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            using (var fs = new FileStream($"{_directory}/{report.TextName}_ConcordanceReport.txt",
                       FileMode.OpenOrCreate))
            {
                using (var writer = new StreamWriter(fs))
                {
                    char prevFirstChar = ' ';

                    foreach (var item in report.Items)
                    {
                        if (item.FirstChar != prevFirstChar)
                        {
                            if (prevFirstChar != ' ')
                            {
                                writer.WriteLine();
                            }

                            prevFirstChar = item.FirstChar;
                            writer.WriteLine(item.FirstChar.ToString());
                        }

                        writer.WriteLine(item.ToString());
                    }
                }
            }
        }
    }
}
