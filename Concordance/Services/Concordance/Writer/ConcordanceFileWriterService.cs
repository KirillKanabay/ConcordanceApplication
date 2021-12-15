using System.IO;
using Concordance.Helpers.Logger;
using Concordance.Model;
using Microsoft.Extensions.Configuration;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceFileWriterService : IConcordanceWriterService
    {
        private readonly string _directory;
        private readonly ILogger _logger;

        public ConcordanceFileWriterService(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            _directory = configuration["Output"];
        }

        public ServiceResult Write(ConcordanceReport report)
        {

            if (!Directory.Exists(_directory))
            {
                _logger.Information($"Creating output directory: {_directory}");
                Directory.CreateDirectory(_directory);
            }

            string fileName = $"{_directory}/{report.TextName}_ConcordanceReport.txt";

            _logger.Information($"Start writing concordance report in file: {fileName}");
            //todo: check report to null
            try
            {
                using (var fs = new FileStream($"", FileMode.OpenOrCreate))
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
                _logger.Error($"File {fileName} doesn't exists or being used by another process");

                return new ServiceResult()
                {
                    IsSuccess = false,
                    Error = $"File {fileName} doesn't exists or being used by another process",
                };
            }

            return new ServiceResult()
            {
                IsSuccess = true
            };
        }
    }
}
