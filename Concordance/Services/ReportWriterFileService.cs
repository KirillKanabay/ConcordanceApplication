using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Concordance.Services
{
    public class ReportWriterFileService:IReportWriterService
    {
        private readonly IConfiguration _configuration;
        private readonly string _path;
        public ReportWriterFileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _path = _configuration["outputPath"];
        }
        public void Write(IReportService reportService)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            using var fs = new FileStream($"{_path}/{reportService.Text.Name}_ConcordanceReport.txt", FileMode.OpenOrCreate);
            using var writer = new StreamWriter(fs);
            writer.Write(reportService.ToString());
        }
    }
}
