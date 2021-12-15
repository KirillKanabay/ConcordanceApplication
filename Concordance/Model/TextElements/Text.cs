using System.Collections.Generic;
using System.Text;

namespace Concordance.Model.TextElements
{
    /// <summary>
    /// Класс текста
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Название текста
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Страницы текста
        /// </summary>
        public IEnumerable<Page> Pages { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in Pages)
            {
                sb.Append(page);
            }

            return sb.ToString();
        }
    }
}
