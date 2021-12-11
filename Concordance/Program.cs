using System;
using System.IO;
using Concordance.Configurations;
using Concordance.Helpers;
using Concordance.Interfaces;
using Concordance.View;
using Microsoft.Extensions.Configuration;

namespace Concordance
{
    class Program
    {
        private static IConfiguration _configuration;
        private static IConfigurationParser _textInfoParser;
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
            _textInfoParser = new ConfigurationParser(_configuration);
            _concordanceView = new ConcordanceView(_textInfoParser);
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
