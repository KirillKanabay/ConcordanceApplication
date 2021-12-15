using Concordance.Model.Options;
using Microsoft.Extensions.Configuration;

namespace Concordance.Services.Configurations
{
    public class ConfigurationParserService:IConfigurationParserService
    {
        private readonly IConfiguration _configuration;
        public ConfigurationParserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public TextOptions GetTextOptions()
        {
            var section = _configuration.GetSection("TextOptions");
            var textOptions = section.Get<TextOptions>();

            return textOptions;
        }

        public string GetOutputDirectory()
        {
            return _configuration["outputDirectory"];
        }
    }
}
