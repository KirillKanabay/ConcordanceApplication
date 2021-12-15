using System;
using System.IO;
using System.Threading.Tasks;
using Concordance.Concordance;
using Concordance.Configurations;
using Concordance.Helpers;
using Concordance.Model;
using Concordance.Parser;
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
            ParserResult parserResult = null;

            FileStream fs = null;

            try
            {
                fs = new FileStream("text.txt", FileMode.Open, FileAccess.Read);
                using (var stp = new TextParser(fs, 3))
                {
                    parserResult = await stp.Parse();
                }
            }
            catch (Exception ex)
            {
                ConsoleExtensions.WriteLineError(ex.Message);
            }
            finally
            {
                fs?.DisposeAsync();
            }
            

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
