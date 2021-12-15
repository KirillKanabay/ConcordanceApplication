using System.IO;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.Model.Options;
using Concordance.Services.Concordance;
using Concordance.Services.Configurations;
using Concordance.Services.Parser;
using Concordance.Validation;
using Concordance.View;
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

            serviceCollection.AddSingleton(GetConfiguration());
            serviceCollection.AddSingleton<IConfigurationParserService, ConfigurationParserService>();
            
            serviceCollection.AddTransient<EntryPoint>();

            serviceCollection.AddScoped<IValidator<TextOptions>, TextOptionsValidator>();

            serviceCollection.AddScoped<ITextParserService, TextParserService>();
            
            serviceCollection.AddTransient<IView, ConcordanceView>();

            serviceCollection.AddSingleton<IStateGenerator, StateGenerator>();
            serviceCollection.AddTransient<IFiniteStateMachine, FiniteStateMachine>();
            serviceCollection.AddTransient<IFiniteStateMachineBuilder, FiniteStateMachineBuilder>();

            serviceCollection.AddScoped<IConcordanceReportService, ConcordanceReportService>();

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
