using System;
using System.Collections.Generic;
using System.Text;

namespace Concordance.Helpers
{
    public static class ConsoleExtensions
    {
        public static void WriteLineError(string error)
        {
            WriteLineWithColor(error, ConsoleColor.Red);
        }

        public static bool CheckContinue(string message)
        {
            Console.Write(message);
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return char.ToLower(key) == 'y';
        }

        public static void WriteLineWithColor(string message, ConsoleColor foregroundColor)
        {
            var defaultConsoleForeground = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;

            Console.WriteLine(message);

            Console.ForegroundColor = defaultConsoleForeground;
        }
    }
}
