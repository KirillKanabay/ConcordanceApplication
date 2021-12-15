using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public interface IConcordanceWriter
    {
        public void Write(ConcordanceReport report);
    }
}
