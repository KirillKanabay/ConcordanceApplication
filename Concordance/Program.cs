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
        static void Main(string[] args)
        {
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<EntryPoint>()?.Run();
        }
    }
}
