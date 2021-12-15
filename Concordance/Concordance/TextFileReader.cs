using System.IO;
using Concordance.Model;

namespace Concordance.Concordance
{
    class TextFileReader
    {
        private readonly string _filePath;
        private readonly int _pageSize;
        public TextFileReader(string filePath, int pageSize)
        {
            _filePath = filePath;
            _pageSize = pageSize;
        }
        
        public Text Read()
        {
            using var fs = new FileStream(_filePath, FileMode.Open);
            using var reader = new StreamReader(fs);

            var plainText = reader.ReadToEnd();

            //Text text = new Text(Path.GetFileNameWithoutExtension(_filePath), plainText, _pageSize);

            return null;
        }
    }
}
