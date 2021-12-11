using System.Collections.Generic;
using Concordance.Interfaces;
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

        public IEnumerable<string> GetInputFilePaths()
        {
            List<string> inputFiles = new List<string>();
            
            var section = _configuration.GetSection("inputFiles");
            foreach (var child in section.GetChildren())
            {
                inputFiles.Add(child.Value);
            }

            return inputFiles;
        }

        public string GetOutputDirectory()
        {
            return _configuration["outputDirectory"];
        }

        public int GetPageSize()
        {
            return int.Parse(_configuration["pageSize"]);
        }
    }
}
