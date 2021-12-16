using System;
using Concordance.Constants;
using Concordance.Helpers;

namespace Concordance.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        public void Information(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorLogConstants.LogInfoIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Info, ConsoleColor.White, ConsoleColor.Blue);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }

        public void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorLogConstants.LogErrorIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Error, ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }

        public void Success(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(ErrorLogConstants.LogSuccessIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Success, ConsoleColor.White, ConsoleColor.DarkGreen);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }
    }
}
