using Concordance.Model;
using Concordance.Model.Options;

namespace Concordance.Parser
{
    public interface ITextParser
    {
        ParserResult Parse(TextOptions options);
    }
}