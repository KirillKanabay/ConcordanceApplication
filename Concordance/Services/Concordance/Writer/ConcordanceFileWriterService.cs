using System.IO;
using Concordance.Constants;
using Concordance.Infrastructure.Logger;
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
            _directory = configuration[DataConstants.ConfigurationOutputDirSection];
        }

        public ServiceResult Write(ConcordanceReport report)
        {
            if (report == null)
            {
                _logger.Warning(LogConstants.ConcordanceReportForWritingIsNull);
                
                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = LogConstants.ConcordanceReportForWritingIsNull,
                };
            }
            
            if (string.IsNullOrWhiteSpace(_directory))
            {
                _directory = DataConstants.DefaultDir;
            }

            if (!Directory.Exists(_directory))
            {
                _logger.Information($"{LogConstants.StartCreatingDirectory} {_directory}");
                Directory.CreateDirectory(_directory);
            }

            string fileName = $"{_directory}/{report.TextName}_{DataConstants.ReportFileName}";

            _logger.Information($"{LogConstants.StartWritingReportToFile} {fileName}");

            try
            {
                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var writer = new StreamWriter(fs))
                    {
                        char prevFirstChar = DataConstants.EmptyChar;

                        foreach (var item in report.Items)
                        {
                            if (item.FirstChar != prevFirstChar)
                            {
                                if (prevFirstChar != DataConstants.EmptyChar)
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
            catch (IOException ex)
            {
                _logger.Error($"{LogConstants.FileNotExistsOrUsedByAnotherProcess} {fileName}. Exception message: {ex.Message}");

                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = $"{LogConstants.FileNotExistsOrUsedByAnotherProcess} {fileName}",
                };
            }

            _logger.Information($"{LogConstants.WroteReportToFile} {fileName}");

            return new ServiceResult()
            {
                IsSuccess = true
            };
        }
    }
}
