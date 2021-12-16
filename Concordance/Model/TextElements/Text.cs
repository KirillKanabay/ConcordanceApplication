using System.Collections.Generic;
using System.Text;

namespace Concordance.Model.TextElements
{
    public class Text
    {
        public string Name { get; set; }
        public IEnumerable<Page> Pages { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var page in Pages)
            {
                sb.Append(page);
            }

            return sb.ToString();
        }
    }
}
