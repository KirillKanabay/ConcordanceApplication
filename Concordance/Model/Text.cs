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
        public string Name { get; }
        /// <summary>
        /// Текст в исходном формате
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Страницы текста
        /// </summary>
        public IEnumerable<Page> Pages { get; }

        /// <summary>
        /// Размер одной страницы
        /// </summary>
        public int PageSize { get; }

        public Text(string name, string content, int pageSize)
        {
            Name = name;
            Content = content;
            PageSize = pageSize;

            Pages = Page.Split(Content, PageSize);
        }
    }
}
