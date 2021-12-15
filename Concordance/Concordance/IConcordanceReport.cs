using System.Collections.Generic;
using Concordance.Model;

namespace Concordance.Concordance
{
    public interface IConcordanceReport
    {
        IDictionary<Word, ConcordanceReportItem> ReportList { get; }
        Text Text { get; }
        void MakeReport();
        string ToString();
    }
}