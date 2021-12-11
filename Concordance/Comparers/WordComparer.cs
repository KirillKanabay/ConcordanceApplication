using System;
using System.Collections.Generic;
using Concordance.Model;

namespace Concordance.Comparers
{
    public class WordComparer : IComparer<Word>
    {
        public int Compare(Word x, Word y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return string.Compare(x.Content, y.Content, StringComparison.Ordinal);
        }
    }
}