using System.IO;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.Helpers.Logger;
using Concordance.Model.Options;
using Concordance.Services.Concordance;
using Concordance.Services.Concordance.Writer;
using Concordance.Services.Configurations;
using Concordance.Services.Parser;
using Concordance.Validation;
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
            serviceCollection.AddScoped<IConfigurationParserService, ConfigurationParserService>();
            serviceCollection.AddScoped<IValidator<TextOptions>, TextOptionsValidator>();
            serviceCollection.AddScoped<ITextParserService, TextParserService>();
            serviceCollection.AddSingleton<IStateGenerator, StateGenerator>();
            serviceCollection.AddTransient<IFiniteStateMachine, FiniteStateMachine>();
            serviceCollection.AddTransient<IFiniteStateMachineBuilder, FiniteStateMachineBuilder>();
            serviceCollection.AddScoped<IConcordanceReportService, ConcordanceReportService>();
            serviceCollection.AddScoped<IConcordanceWriterService, ConcordanceFileWriterService>();

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
