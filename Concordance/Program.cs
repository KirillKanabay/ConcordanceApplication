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
using Microsoft.Extensions.DependencyInjection;

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
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<EntryPoint>()?.Run();
        }
    }
}
