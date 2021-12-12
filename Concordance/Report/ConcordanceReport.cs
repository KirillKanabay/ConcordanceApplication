using System;
using System.Collections.Generic;
using System.Text;
using Concordance.Interfaces;
using Concordance.Model;

namespace Concordance.Report
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
        //     foreach (var word in page.Words)
        //     {
        //         if (!ReportList.ContainsKey(word))
        //         {
        //             ReportList.Add(word, new ConcordanceReportItem(word));
        //         }
        //
        //         ReportList[word].AddPage(page.Number);
        //     }
        throw new NotImplementedException();
    }

        public override string ToString()
        {
            var sb = new StringBuilder();
            char prevFirstChar = ' ';
            
            foreach (var item in ReportList.Values)
            {
                if (item.Word.FirstChar != prevFirstChar)
                {
                    if (prevFirstChar != ' ')
                    {
                        sb.AppendLine();
                    }

                    prevFirstChar = item.Word.FirstChar;
                    sb.AppendLine(char.ToUpper(prevFirstChar).ToString());
                }

                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}
