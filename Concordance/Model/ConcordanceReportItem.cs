using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordance.Model.TextElements;

namespace Concordance.Model
{
    public class ConcordanceReportItem
    {
        public char FirstChar => char.ToUpper(Word.FirstChar);
        public Word Word { get; set; }
        public int Count { get; set; }
        public ICollection<int> PageNumbers { get; set; }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Word.Content.ToLower())
                .Append(new string('.', 60 - Word.Content.Length))
                .Append(Count)
                .Append(" : ")
                .Append(string.Join(' ', PageNumbers));

            return sb.ToString();
        }
    }
}
