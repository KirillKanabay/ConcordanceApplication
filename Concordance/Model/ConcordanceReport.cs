using System.Collections.Generic;

namespace Concordance.Model
{
    public class ConcordanceReport
    {
        public string TextName { get; set; }
        public IEnumerable<ConcordanceReportItem> Items { get; set; }
    }
}
