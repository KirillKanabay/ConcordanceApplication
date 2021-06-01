using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Concordance.Model
{
    /// <summary>
    /// Класс слова
    /// </summary>
    public class Word : IComparer, IComparable
    {
        /// <summary>
        /// Содержимое слова
        /// </summary>
        public string Content { get; }
        public char FirstChar => Content[0];

        public Word(string content)
        {
            Content = content;
        }

        //public Word AddPage(int page)
        //{
        //    if (PageNumbers.FirstOrDefault(p => p == page) == default)
        //        PageNumbers.Add(page);

        //    return this;
        //}

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

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(Value)
        //        .Append(new string('.', 60 - Value.Length))
        //        .Append(Count)
        //        .Append(" : ")
        //        .Append(string.Join(' ', PageNumbers));

        //    return sb.ToString();
        //}
    
    }
}
