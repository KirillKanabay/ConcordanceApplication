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
                Error(LogConstants.LogInfoIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Info, ConsoleColor.White, ConsoleColor.Blue);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }

        public void Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(LogConstants.LogErrorIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Error, ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }

        public void Warning(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Error(LogConstants.LogWarningIsEmpty);
                return;
            }
            ConsoleExtensions.WriteWithColor(LogConstants.Warning, ConsoleColor.White, ConsoleColor.DarkYellow);
            Console.WriteLine($" {message} {Environment.NewLine}");
        }
    }
}
