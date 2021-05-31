using System.Collections.Generic;
using Concordance.Model;

namespace Concordance.Services
{
    public interface IReportService
    {
        SortedList<char, List<Word>> ReportList { get; }
        Text Text { get; }
        void MakeReport();
        string ToString();
    }
}