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
        /// Слова страницы
        /// </summary>
        public IEnumerable<Sentence> Sentences { get; }
        /// <summary>
        /// Номер страницы
        /// </summary>
        public int Number { get; }
    }
}
