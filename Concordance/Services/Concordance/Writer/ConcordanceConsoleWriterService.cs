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
            _logger.Information(InfoLogConstants.StartWritingReportToConsole);

            if (report == null)
            {
                _logger.Error(ErrorLogConstants.ConcordanceReportForWritingIsNull);
                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = ErrorLogConstants.ConcordanceReportForWritingIsNull,
                };
            }

            char prevFirstChar = CharConstants.Empty;

            foreach (var item in report.Items)
            {
                if (item.FirstChar != prevFirstChar)
                {
                    if (prevFirstChar != CharConstants.Empty)
                    {
                        Console.WriteLine();
                    }

                    prevFirstChar = item.FirstChar;
                    Console.WriteLine(item.FirstChar.ToString());
                }

                Console.WriteLine(item.ToString());
            }

            _logger.Success(SuccessLogConstants.WroteReportToConsole);
            
            return new ServiceResult()
            {
                IsSuccess = true,
            };
        }
    }
}
