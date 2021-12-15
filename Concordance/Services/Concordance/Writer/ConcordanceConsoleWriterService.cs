using System;
using Concordance.Constants;
using Concordance.Helpers.Logger;
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
            _logger.Information(InfoConstants.StartWritingReportToConsole);

            if (report == null)
            {
                _logger.Error(ErrorConstants.ConcordanceReportIsNull);
                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = ErrorConstants.ConcordanceReportIsNull,
                };
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

            _logger.Information(InfoConstants.EndWritingReportToConsole);
            
            return new ServiceResult()
            {
                IsSuccess = true,
            };
        }
    }
}
