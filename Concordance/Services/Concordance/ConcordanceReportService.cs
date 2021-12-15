using System.Collections.Generic;
using System.Linq;
using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Services.Concordance
{
    public class ConcordanceReportService : IConcordanceReportService
    {
        public ConcordanceReport MakeReport(Text text)
        {
            var report = new SortedList<Word, ConcordanceReportItem>();

            foreach (var page in text.Pages)
            {
                var words = page.Sentences
                    .SelectMany(p => p.SentenceElements.Where(se => se is Word))
                    .Select(se => se as Word);

                foreach (var word in words)
                {
                    if (!report.ContainsKey(word))
                    {
                        report.Add(word, new ConcordanceReportItem(word));
                    }

                    report[word].AddPage(page.Number);
                }
            }

            return new ConcordanceReport()
            {
                Items = report.Values
            };
        }
    }
}