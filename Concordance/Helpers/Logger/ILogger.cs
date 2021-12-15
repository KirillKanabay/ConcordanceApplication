namespace Concordance.Helpers.Logger
{
    public interface ILogger
    {
        void Information(string message);
        void Error(string message);
    }
}
