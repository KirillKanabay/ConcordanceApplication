using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordance.Model
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
        public int Number { get; }

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
