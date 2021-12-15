using Microsoft.Extensions.DependencyInjection;

namespace Concordance
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<AppHost>()?.Run();
        }
    }
}
