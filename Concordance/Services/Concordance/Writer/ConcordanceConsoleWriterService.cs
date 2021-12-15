using System;
using Concordance.Helpers.Logger;
using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceConsoleWriterService : IConcordanceWriter
    {
        private readonly ILogger _logger;
        public ConcordanceConsoleWriterService(ILogger logger)
        {
            _logger = logger;
        }

        public void Write(ConcordanceReport report)
        {
            _logger.Information("Start writing concordance report.");

            if (report == null)
            {
                _logger.Error("ConcordanceConsoleWriterService: report can't be null");
            }

            char prevFirstChar = ' ';

            foreach (var item in report.Items)
            {
                if (item.FirstChar != prevFirstChar)
                {
                    if (prevFirstChar != ' ')
                    {
                        Console.WriteLine();
                    }

                    prevFirstChar = item.FirstChar;
                    Console.WriteLine(item.FirstChar.ToString());
                }

                Console.WriteLine(item.ToString());
            }
        }
    }
}
