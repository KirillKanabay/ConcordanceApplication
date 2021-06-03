using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordance.Interfaces;
using Concordance.Model;

namespace Concordance.Report
{
    public class ConcordanceReport : IConcordanceReport
    {
        public IDictionary<char, ICollection<ConcordanceReportItem>> ReportList { get; }
        public Text Text { get; }

        public ConcordanceReport(Text text)
        {
            Text = text;
            ReportList = new SortedList<char, ICollection<ConcordanceReportItem>>();
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
            foreach (var word in page.Words)
            {
                var firstChar = word.FirstChar;
                //Если нет буквы в списке, то добавляем букву и слово
                if (!ReportList.ContainsKey(firstChar))
                {
                    ReportList.Add(firstChar, new List<ConcordanceReportItem>()
                    {
                        new ConcordanceReportItem(word).AddPage(page.Number)
                    });
                }
                else
                {
                    var foundWord = ReportList[firstChar].FirstOrDefault(cri => cri.Word.CompareTo(word) == 0);
                    //Если в списке есть уже слово, то добавляем страницу и увеличиваем количество встреч слова в тексте
                    if (foundWord != null)
                    {
                        foundWord.AddPage(page.Number);
                    }
                    else
                    {
                        //Если слова нет, то добавляем новое слово
                        ReportList[firstChar].Add(new ConcordanceReportItem(word).AddPage(page.Number));
                    }
                }

                ((List<ConcordanceReportItem>)ReportList[firstChar]).Sort();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var key in ReportList.Keys)
            {
                sb.AppendLine(char.ToUpper(key).ToString());
                foreach (var word in ReportList[key])
                {
                    sb.AppendLine(word.ToString());
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
