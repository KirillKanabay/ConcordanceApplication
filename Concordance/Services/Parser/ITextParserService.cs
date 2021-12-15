using Concordance.Model;
using Concordance.Model.Options;

namespace Concordance.Services.Parser
{
    public interface ITextParserService
    {
        ParserResult Parse(TextOptions options);
    }
}