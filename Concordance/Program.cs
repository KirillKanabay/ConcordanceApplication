using System;
using System.IO;
using System.Threading.Tasks;
using Concordance.Configurations;
using Concordance.Helpers;
using Concordance.Interfaces;
using Concordance.IO;
using Concordance.Report;
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
        
         

        static async Task Main(string[] args)
        {
            FileWordParser fwp = new FileWordParser("text.txt", 5);
            var parserResult = await fwp.Parse();

            if (!parserResult.IsSuccess)
            {
                ConsoleExtensions.WriteLineError(parserResult.Error);
            }

            var report = new ConcordanceReport(parserResult.Text);
            report.MakeReport();

            Console.WriteLine(report);
        }
    }
}
