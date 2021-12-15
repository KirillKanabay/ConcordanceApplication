namespace Concordance.Helpers.Logger
{
    internal interface ILogger
    {
        void Information(string message);
        void Error(string message);
    }
}
