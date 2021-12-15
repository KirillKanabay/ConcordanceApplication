using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public interface IConcordanceWriterService
    {
        public ServiceResult Write(ConcordanceReport report);
    }
}
