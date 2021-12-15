using Concordance.Model;
using Concordance.Model.Options;
using Concordance.Model.TextElements;

namespace Concordance.Services.Parser
{
    public interface ITextParserService
    {
        ServiceResult<Text> Parse(TextOptions options);
    }
}