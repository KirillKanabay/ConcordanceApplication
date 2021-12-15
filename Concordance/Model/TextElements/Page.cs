using System.Collections.Generic;
using System.Text;

namespace Concordance.Model.TextElements
{
    /// <summary>
    /// Класс страницы
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Слова страницы
        /// </summary>
        public IEnumerable<Sentence> Sentences { get; set; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Number { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var sentence in Sentences)
            {
                sb.Append(sentence);
            }

            return sb.ToString();
        }
    }
}
