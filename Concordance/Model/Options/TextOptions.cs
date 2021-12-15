namespace Concordance.Model.Options
{
    public class TextOptions
    {
        public string Path { get; set; }
        public int PageSize { get; set; }
        public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);
    }
}
