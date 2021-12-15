using Concordance.Model.Options;
using Microsoft.Extensions.Configuration;

namespace Concordance.Configurations
{
    public class ConfigurationParser:IConfigurationParser
    {
        private readonly IConfiguration _configuration;
        public ConfigurationParser(IConfiguration configuration)
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
