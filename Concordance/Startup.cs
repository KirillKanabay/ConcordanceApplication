using System.IO;
using Concordance.Configurations;
using Concordance.FSM;
using Concordance.FSM.Builder;
using Concordance.FSM.States;
using Concordance.Model.Options;
using Concordance.Parser;
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
            serviceCollection.AddSingleton<IConfigurationParser, ConfigurationParser>();
            
            serviceCollection.AddTransient<EntryPoint>();

            serviceCollection.AddScoped<IValidator<TextOptions>, TextOptionsValidator>();

            serviceCollection.AddScoped<ITextParser, TextParser>();
            
            serviceCollection.AddTransient<IView, ConcordanceView>();

            serviceCollection.AddSingleton<IStateGenerator, StateGenerator>();
            serviceCollection.AddTransient<IFiniteStateMachine, FiniteStateMachine>();
            serviceCollection.AddTransient<IFiniteStateMachineBuilder, FiniteStateMachineBuilder>();

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
