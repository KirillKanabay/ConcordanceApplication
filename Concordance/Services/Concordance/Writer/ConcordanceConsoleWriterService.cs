using System;
using Concordance.Constants;
using Concordance.Infrastructure.Logger;
using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceConsoleWriterService : IConcordanceWriterService
    {
        private readonly ILogger _logger;
        public ConcordanceConsoleWriterService(ILogger logger)
        {
            _logger = logger;
        }

        public ServiceResult Write(ConcordanceReport report)
        {
            _logger.Information(LogConstants.StartWritingReportToConsole);

            if (report == null)
            {
                _logger.Warning(LogConstants.ConcordanceReportForWritingIsNull);
                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = LogConstants.ConcordanceReportForWritingIsNull,
                };
            }

            char prevFirstChar = DataConstants.EmptyChar;

            foreach (var item in report.Items)
            {
                if (item.FirstChar != prevFirstChar)
                {
                    if (prevFirstChar != DataConstants.EmptyChar)
                    {
                        Console.WriteLine();
                    }

                    prevFirstChar = item.FirstChar;
                    Console.WriteLine(item.FirstChar.ToString());
                }

                Console.WriteLine(item.ToString());
            }

            _logger.Information(LogConstants.WroteReportToConsole);
            
            return new ServiceResult()
            {
                IsSuccess = true,
            };
        }
    }
}
