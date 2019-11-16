using System;
using OAST.Project1.Common.Extensions;
using OAST.Project1.Models.Common;

namespace OAST.Project1.Helpers
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

        public static void PrintPickedScenario(MenuOptions menuOptions)
        {
            SetConsoleColor(ConsoleColor.DarkYellow);
            Console.WriteLine("Picked scenario: ");
            Console.WriteLine($"Algorithm: {menuOptions.AlgorithmType}");
            Console.WriteLine($"Problem type: {menuOptions.ProblemType}");
            Console.WriteLine($"Network: {Extensions.GetFileName(menuOptions.FileName)}");
            SetConsoleColor(ConsoleColor.DarkCyan);
            Console.WriteLine(Environment.NewLine);
        }
    }
}
