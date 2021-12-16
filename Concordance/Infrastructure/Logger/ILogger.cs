namespace Concordance.Infrastructure.Logger
{
    public interface ILogger
    {
        void Information(string message);
        void Error(string message);
        void Success(string message);
    }
}
