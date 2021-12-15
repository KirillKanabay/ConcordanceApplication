﻿using System;
using System.Collections.Generic;
using Concordance.Model.TextElements;

namespace Concordance.Model
{
    /// <summary>
    /// Класс слова
    /// </summary>
    public class Word : BaseSentenceElement, IComparable<Word>
    {
        /// <summary>
        /// Первая буква слова
        /// </summary>
        public char FirstChar => Content[0];

        public int CompareTo(Word other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            if (ReferenceEquals(null, this)) return -1;
            return string.Compare(this.Content, other.Content, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Content.ToLower().GetHashCode();
        }
    }
}