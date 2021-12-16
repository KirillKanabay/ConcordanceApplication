using System.Collections.Generic;
using System.Text;

namespace Concordance.Model.TextElements
{
    public class Page
    {
        public IEnumerable<Sentence> Sentences { get; set; }
        public int Number { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var sentence in Sentences)
            {
                sb.Append(sentence);
            }

            return sb.ToString();
        }
    }
}