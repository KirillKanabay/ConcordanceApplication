using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordance.Model
{
    public class Word:IComparer, IComparable
    {
        public string Value { get; }
        public char FirstChar { get; }
        public int Count { get; set; }
        public List<int> PageNumbers { get; }

        public Word(string value, int page)
        {
            Value = value;
            FirstChar = value[0];
            Count++;
            PageNumbers = new List<int>();
            AddPage(page);
        }

        public Word AddPage(int page)
        {
            if (PageNumbers.FirstOrDefault(p => p == page) == default)
                PageNumbers.Add(page);

            return this;
        }

        public int Compare(object? x, object? y)
        {
            var w1 = (Word) x;
            var w2 = (Word) y;

            return string.Compare(w1?.Value, w2?.Value, StringComparison.CurrentCultureIgnoreCase);
        }
        public int CompareTo(object? obj)
        {
            return Compare(this, obj);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Value)
                .Append(new string('.', 60 - Value.Length))
                .Append(Count)
                .Append(" : ")
                .Append(string.Join(' ', PageNumbers));

            return sb.ToString();
        }
    
    }
}
