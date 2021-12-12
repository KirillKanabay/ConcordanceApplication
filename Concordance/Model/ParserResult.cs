using System.Collections.Generic;

namespace Concordance.Model
{
    public class ParserResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<Word> Words { get; set; }
        public string Error { get; set; }
    }
}