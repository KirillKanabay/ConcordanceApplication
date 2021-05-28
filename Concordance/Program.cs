using System.IO;
using Microsoft.Extensions.Configuration;

namespace Concordance
{
    class Program
    {
        private static IConfiguration _configuration;

        private static void GetConfigurations()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config.json", optional: false, reloadOnChange: true);
            _configuration = configurationBuilder.Build();
        }

        private static void InitDependences()
        {
            
        }
        
        static void Main(string[] args)
        {
            GetConfigurations();
            InitDependences();
        }
    }
}
