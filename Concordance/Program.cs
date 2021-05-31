using System;
using System.IO;
using System.Text;
using Concordance.Helpers;
using Concordance.Services;
using Concordance.View;
using Microsoft.Extensions.Configuration;

namespace Concordance
{
    class Program
    {
        private static IConfiguration _configuration;
        private static ITextReaderService _textReaderService;
        private static IReportWriterService _reportWriterService;
        private static ConcordanceView _concordanceView;
        private static void GetConfigurations()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json", optional: false, reloadOnChange: true);
            _configuration = configurationBuilder.Build();
        }
        
        private static void InitDependencies()
        {
            _textReaderService = new TextReaderFromConfigService(_configuration);
            _reportWriterService = new ReportWriterFileService(_configuration);
            _concordanceView = new ConcordanceView(_textReaderService, _reportWriterService);
        }
        
         

        static void Main(string[] args)
        {
            try
            {
                GetConfigurations();
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Ошибка обработки файла конфигурации : {e.Message}");
                return;
            }
            
            InitDependencies();
            _concordanceView.Show();
        }
    }
}
