using System.Collections.Generic;

namespace Concordance.Model
{
    public class Text
    {
        public string Name { get; set; }
        public int PageSize { get; set; }
        public List<string> Pages { get; set; }

        public Text()
        {
            Pages = new List<string>();
        }
    }
}
