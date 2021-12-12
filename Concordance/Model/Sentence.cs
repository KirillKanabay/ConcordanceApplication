using System.Collections.Generic;
using System.Text;

namespace Concordance.Model
{
    public class Sentence
    {
        public IEnumerable<BaseSentenceElement> SentenceElements { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var baseSentenceElement in SentenceElements)
            {
                sb.Append(baseSentenceElement);
            }

            return sb.ToString();
        }
    }
}
