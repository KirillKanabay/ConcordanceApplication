using System;

namespace Concordance.Model.TextElements
{
    public class Word : BaseSentenceElement, IComparable<Word>
    {
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