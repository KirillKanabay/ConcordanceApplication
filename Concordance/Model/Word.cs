using System;

namespace Concordance.Model
{
    /// <summary>
    /// Класс слова
    /// </summary>
    public class Word : BaseSentenceElement
    {
        /// <summary>
        /// Первая буква слова
        /// </summary>
        public char FirstChar => Content[0];
    }
}
