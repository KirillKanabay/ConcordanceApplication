using System.Threading.Tasks;
using Concordance.Model;

namespace Concordance.Parser
{
    public interface ITextParser
    {
        Task<ParserResult> Parse();
    }
}