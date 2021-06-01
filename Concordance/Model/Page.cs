using System.Collections.Generic;

namespace Concordance.Model
{
    /// <summary>
    /// Класс страницы
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Содержимое страницы
        /// </summary>
        public string Content { get; }
        /// <summary>
        /// Слова страницы
        /// </summary>
        public ICollection<Word> Words { get; }
        /// <summary>
        /// Размер страницы в строках
        /// </summary>
        public int PageSize { get; }
        public Page(string content, int pageSize)
        {
            Content = content;
            PageSize = pageSize;

        }
    }
}
