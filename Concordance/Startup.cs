using System.IO;
using Concordance.Configurations;
using Concordance.Parser;
using Concordance.View;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concordance
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(GetConfiguration());

            serviceCollection.AddSingleton<IConfigurationParser, ConfigurationParser>();
            serviceCollection.AddTransient<EntryPoint>();
            serviceCollection.AddScoped<ITextParser, TextParser>();
            serviceCollection.AddTransient<IView, ConcordanceView>();
            
            return serviceCollection;
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            return configurationBuilder.Build();
        }
    }
}
