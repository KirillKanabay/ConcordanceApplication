using Concordance.View;
using Microsoft.Extensions.DependencyInjection;

namespace Concordance
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<EntryPoint>();
            serviceCollection.AddTransient<IView, ConcordanceView>();
            return serviceCollection;
        }
    }
}
