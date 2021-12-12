using System.Collections.Generic;
using System.Linq;

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
        public string Name { get; set; }

        /// <summary>
        /// Страницы текста
        /// </summary>
        public IEnumerable<Page> Pages { get; set; }
        
    }
}
