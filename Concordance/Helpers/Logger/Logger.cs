using System;

namespace Concordance.Helpers.Logger
{
    public class Logger : ILogger
    {
        public void Information(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error("Logger information message can't be whitespace or null");
            }
            Console.WriteLine($"INFO: {message} \n");
        }

        public void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ConsoleExtensions.WriteLineError($"ERROR: Logger error message can't be whitespace or null\n");
            }
            ConsoleExtensions.WriteLineError($"ERROR: {message} \n");    
        }
    }
}
