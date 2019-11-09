using System;

namespace OAST.Project1
{
    public static class ConsoleHelpers
    {
        public static void SetConsoleColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public static void ClearConsole()
        {
            Console.Clear();
        }
        public static void DisplayError<T>(T value)
        {
            SetConsoleColor(ConsoleColor.Red);
            Console.WriteLine($"{value} cannot be applied. Please pass a correct value");
            SetConsoleColor(ConsoleColor.DarkCyan);
        }
    }
}
