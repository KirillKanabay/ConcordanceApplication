using System;

namespace Concordance.Helpers
{
    public static class ConsoleExtensions
    {
        public static void WriteWithColor(string message, ConsoleColor foregroundColor, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            var defaultConsoleForeground = Console.ForegroundColor;
            var defaultConsoleBackground = Console.BackgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            
            Console.Write(message);

            Console.ForegroundColor = defaultConsoleForeground;
            Console.BackgroundColor = defaultConsoleBackground;

        }

        public static void WriteLineError(string error)
        {
            WriteLineWithColor(error, ConsoleColor.Red);
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
