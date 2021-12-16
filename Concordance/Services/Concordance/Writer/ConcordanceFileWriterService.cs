using System.IO;
using Concordance.Constants;
using Concordance.Helpers.Logger;
using Concordance.Model;
using Microsoft.Extensions.Configuration;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceFileWriterService : IConcordanceWriterService
    {
        private string _directory;
        private readonly ILogger _logger;

        public ConcordanceFileWriterService(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _directory = configuration[ConfigurationConstants.OutputConcordanceDirSection];
        }

        public ServiceResult Write(ConcordanceReport report)
        {
            if (report == null)
            {
                _logger.Error(ErrorLogConstants.ConcordanceReportForWritingIsNull);
                
                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = ErrorLogConstants.ConcordanceReportForWritingIsNull,
                };
            }
            
            if (string.IsNullOrWhiteSpace(_directory))
            {
                _directory = "concordance_reports";
            }

            if (!Directory.Exists(_directory))
            {
                _logger.Information($"Creating output directory: {_directory}");
                Directory.CreateDirectory(_directory);
            }

            string fileName = $"{_directory}/{report.TextName}_ConcordanceReport.txt";

            _logger.Information($"{InfoLogConstants.StartWritingReportToFile} {fileName}");

            try
            {
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
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
            catch (IOException)
            {
                _logger.Error($"{ErrorLogConstants.FileNotExistsOrUsedByAnotherProcess} {fileName}");

                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = $"{ErrorLogConstants.FileNotExistsOrUsedByAnotherProcess} {fileName}",
                };
            }

            _logger.Success($"{SuccessLogConstants.WroteReportToFile} {fileName}");

            return new ServiceResult()
            {
                IsSuccess = true
            };
        }
    }
}
