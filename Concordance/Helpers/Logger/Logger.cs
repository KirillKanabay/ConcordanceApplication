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
                Error(ErrorLogConstants.LogInfoIsEmpty);
            }
            ConsoleExtensions.WriteWithColor("INFO:", ConsoleColor.White, ConsoleColor.Blue);
            Console.WriteLine($" {message} \n");
        }

        public void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorLogConstants.LogErrorIsEmpty);
            }
            ConsoleExtensions.WriteWithColor("INFO:", ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine($" {message} \n");
        }

        public void Success(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorLogConstants.LogSuccessIsEmpty);
            }
            ConsoleExtensions.WriteWithColor("SUCCESS:", ConsoleColor.White, ConsoleColor.DarkGreen);
            Console.WriteLine($" {message} \n");
        }
    }
}
