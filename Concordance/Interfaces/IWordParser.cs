using System.Collections.Generic;
using System.Threading.Tasks;
using Concordance.Model;

namespace Concordance.Interfaces
{
    public interface IWordParser
    {
        Task<ParserResult> Parse();
    }
}