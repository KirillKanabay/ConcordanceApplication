using System.Collections.Generic;
using Concordance.Model;
using Concordance.Report;

namespace Concordance.Interfaces
{
    public interface IConcordanceReport
    {
        IDictionary<char, ICollection<ConcordanceReportItem>> ReportList { get; }
        Text Text { get; }
        void MakeReport();
        string ToString();
    }
}