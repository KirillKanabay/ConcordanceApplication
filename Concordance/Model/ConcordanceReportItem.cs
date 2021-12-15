using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordance.Model.TextElements;

namespace Concordance.Model
{
    /// <summary>
    /// Элемент отчета
    /// </summary>
    public class ConcordanceReportItem
    {
        /// <summary>
        /// Слово
        /// </summary>
        public Word Word { get; }
        /// <summary>
        /// Количество встреч
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Номера страниц на которых встречается слово
        /// </summary>
        public ICollection<int> PageNumbers { get; }

        public ConcordanceReportItem(Word word)
        {
            Word = word;
            PageNumbers = new List<int>();
        }

        /// <summary>
        /// Метод добавляющий страницу, на которой встречается слово
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ConcordanceReportItem AddPage(int pageNumber)
        {
            if (PageNumbers.FirstOrDefault(p => p == pageNumber) == default)
            {
                PageNumbers.Add(pageNumber);
            }

            Count++;

            return this;
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Word.Content.ToLower())
                .Append(new string('.', 60 - Word.Content.Length))
                .Append(Count)
                .Append(" : ")
                .Append(string.Join(' ', PageNumbers));

            return sb.ToString();
        }
    }
}
