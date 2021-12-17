using System.IO;
using Concordance.Constants;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.FSM.States.Parser;
using Concordance.Infrastructure.Logger;
using Concordance.Model.Options;
using Concordance.Services.Concordance;
using Concordance.Services.Concordance.Writer;
using Concordance.Services.Configurations;
using Concordance.Services.Parser;
using Concordance.Services.Validation;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Concordance
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = GetConfiguration();
            
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<ILogger, Logger>();

            serviceCollection.AddTransient<AppHost>();
            serviceCollection.AddTransient<IFiniteStateMachine, FiniteStateMachine>();
            serviceCollection.AddTransient<IFiniteStateMachineBuilder, FiniteStateMachineBuilder>();
            serviceCollection.AddTransient<ITextParserService, TextParserService>();

            serviceCollection.AddScoped<IStateParser, StateParser>();
            serviceCollection.AddScoped<IConfigurationParserService, ConfigurationParserService>();
            serviceCollection.AddScoped<IValidator<TextOptions>, TextOptionsValidator>();
            serviceCollection.AddScoped<IConcordanceReportService, ConcordanceReportService>();
            serviceCollection.AddScoped<IConcordanceWriterService, ConcordanceConsoleWriterService>();

            return serviceCollection;
        }

        private static IConfiguration GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(DataConstants.ConfigurationPath, optional: false, reloadOnChange: true);
            
            return configurationBuilder.Build();
        }
    }
}
