using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Services.Concordance
{
    public interface IConcordanceReportService
    {
       ServiceResult<ConcordanceReport> Create(Text text);
    }
}