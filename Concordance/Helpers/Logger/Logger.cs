using System;
using Concordance.Constants;

namespace Concordance.Helpers.Logger
{
    public class Logger : ILogger
    {
        public void Information(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorConstants.LogInfoIsEmpty);
            }
            ConsoleExtensions.WriteWithColor("INFO:", ConsoleColor.White, ConsoleColor.Blue);
            Console.WriteLine($" {message} \n");
        }

        public void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorConstants.LogErrorIsEmpty);
            }
            ConsoleExtensions.WriteWithColor("INFO:", ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine($" {message} \n");
        }
    }
}
