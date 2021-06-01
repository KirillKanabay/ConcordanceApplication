using System.Collections.Generic;

namespace Concordance.Model
{
    /// <summary>
    /// Класс текста
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Название текста
        /// </summary>
        public string Name { get; }

        public ICollection<Page> Pages { get; }

        public Text()
        {
            Pages = new List<string>();
        }
    }
}
