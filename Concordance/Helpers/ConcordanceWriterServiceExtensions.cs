using Concordance.Services.Concordance.Writer;
using Microsoft.Extensions.DependencyInjection;

namespace Concordance.Helpers
{
    public static class ConcordanceWriterServiceExtensions
    {
        public static IServiceCollection AddConcordanceFileWriter(this IServiceCollection services,
            string outputDirectory)
        {
            services.AddScoped<IConcordanceWriter, ConcordanceFileWriterService>(_ =>
                new ConcordanceFileWriterService(outputDirectory));

            return services;
        }
        public static IServiceCollection AddConcordanceConsoleWriter(this IServiceCollection services)
        {
            services.AddScoped<IConcordanceWriter, ConcordanceConsoleWriterService>();
            return services;
        }
    }
}
