﻿using System.Collections.Generic;
using System.Linq;
using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Concordance
{
    public class ConcordanceReport : IConcordanceReport
    {
        public IDictionary<Word, ConcordanceReportItem> ReportList { get; }
        public Text Text { get; }

        public ConcordanceReport(Text text)
        {
            Text = text;
            ReportList = new SortedList<Word, ConcordanceReportItem>();
        }

        public void MakeReport()
        {
            foreach (var page in Text.Pages)
            {
                HandleWords(page);
            }
        }
        
        private void HandleWords(Page page)
        {
            var words = page.Sentences
                .SelectMany(p => p.SentenceElements.Where(se => se is Word))
                .Select(se => se as Word);

            foreach (var word in words)
            {
                if (!ReportList.ContainsKey(word))
                {
                    ReportList.Add(word, new ConcordanceReportItem(word));
                }

                ReportList[word].AddPage(page.Number);
            }
        }
    }
}