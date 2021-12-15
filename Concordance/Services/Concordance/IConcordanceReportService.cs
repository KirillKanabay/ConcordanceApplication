using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Services.Concordance
{
    public interface IConcordanceReportService
    {
       ConcordanceReport MakeReport(Text text);
    }
}