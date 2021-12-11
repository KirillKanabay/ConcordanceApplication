using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Concordance.Model
{
    /// <summary>
    /// Класс слова
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Содержимое слова
        /// </summary>
        public string Content { get; }
        
        /// <summary>
        /// Первая буква слова
        /// </summary>
        public char FirstChar => Content[0];
        
        public Word(string content)
        {
            Content = content;
        }

        public static IEnumerable<Word> Split(string plainText)
        {
            var separators = new [] { ",", " ", ".", "(", ")", ":", ";", "-", "!", "?", "_", "—", Environment.NewLine};
            var wordStrings = plainText.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return wordStrings.Select(wordString => new Word(wordString.ToLower())).ToList();
        }

        public int Compare(object? x, object? y)
        {
            var w1 = (Word) x;
            var w2 = (Word) y;

            return string.Compare(w1?.Content, w2?.Content, StringComparison.CurrentCultureIgnoreCase);
        }
        public int CompareTo(object? obj)
        {
            return Compare(this, obj);
        }

        public override int GetHashCode() => Content.GetHashCode();
    }
}
