using System;
using System.Collections.Generic;
using System.Linq;

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
        public IEnumerable<Word> Words { get; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Number { get; }

        public Page(string content, int number)
        {
            Content = content;
            Number = number;
            Words = Word.Split(Content);
        }

        public static IEnumerable<Page> Split(string plainText, int pageSize)
        {
            var pages = new List<Page>();

            var lines = plainText.Split(Environment.NewLine);
            
            for (int idxLine = 0; idxLine < lines.Length; idxLine += pageSize)
            {
                string pageText = string.Concat(lines.Skip(idxLine).Take(pageSize));
                var page = new Page(content: pageText, number: (idxLine / pageSize) + 1);
                pages.Add(page);
            }

            return pages;
        }
    }
}
