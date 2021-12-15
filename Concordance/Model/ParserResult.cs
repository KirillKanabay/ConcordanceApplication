using System.Collections.Generic;
using Concordance.Model.TextElements;

namespace Concordance.Model
{
    public class ParserResult
    {
        public bool IsSuccess { get; set; }
        public Text Text { get; set; }
        public string Error { get; set; }
    }
}