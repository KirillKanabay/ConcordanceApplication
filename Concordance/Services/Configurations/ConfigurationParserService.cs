using Concordance.Constants;
using Concordance.Helpers.Logger;
using Concordance.Model.Options;
using Microsoft.Extensions.Configuration;

namespace Concordance.Services.Configurations
{
    public class ConfigurationParserService:IConfigurationParserService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public ConfigurationParserService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        public TextOptions GetTextOptions()
        {
            var section = _configuration.GetSection(ConfigurationConstants.TextOptionsSection);
            if (!section.Exists())
            {
                _logger.Error(ErrorLogConstants.TextOptionsSectionNotExists);
                return null;
            }

            var textOptions = section.Get<TextOptions>();

            return textOptions;
        }
    }
}
