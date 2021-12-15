using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Services.Concordance
{
    public interface IConcordanceReportService
    {
       ConcordanceReport Create(Text text);
    }
}