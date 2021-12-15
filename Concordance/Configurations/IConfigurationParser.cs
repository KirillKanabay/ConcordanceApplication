using Concordance.Model.Options;

namespace Concordance.Configurations
{
    public interface IConfigurationParser
    {
        TextOptions GetTextOptions();
        string GetOutputDirectory();
    }
}
