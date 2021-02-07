using System;

namespace AkkaNet.MovieStreaming.Irrelevant
{
    public static class ColorConsole
    {
        public static void WriteLine(string message, ConsoleColor color, object[] args = null)
        {
            var tempColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (args != null)
                Console.WriteLine(message, args);
            else
                Console.WriteLine(message);
            Console.ForegroundColor = tempColor;
        }

        public static void WriteLineGreen(string message, object[] args = null) =>
            WriteLine(message, ConsoleColor.Green, args);

        public static void WriteLineRed(string message, object[] args = null) =>
            WriteLine(message, ConsoleColor.Red, args);

        public static void WriteLineYellow(string message, object[] args = null) =>
            WriteLine(message, ConsoleColor.Yellow, args);

    }
}
