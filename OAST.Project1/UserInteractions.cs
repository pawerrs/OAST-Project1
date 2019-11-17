using System;
using OAST.Project1.Common.Enums;
using OAST.Project1.Common.Extensions;
using OAST.Project1.Helpers;

namespace OAST.Project1
{
    public static class UserInteractions
    {
        public static AlgorithmType GetAlgorithmType()
        {
            ConsoleHelpers.ClearConsole();
            while (true)
            {
                Console.WriteLine("Choose an algorithm");
                Console.WriteLine("1. Evolutionary Algorithm");
                Console.WriteLine("2. BruteForce Algorithm");

                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 2 || selectedNumber < 1))
                {
                    ConsoleHelpers.ClearConsole();
                    return (AlgorithmType)selectedNumber;
                }

                ConsoleHelpers.DisplayError(userInput);
            }
        }

        public static void GetVariableFromUser(string question, out float numberToReturn, bool shouldBeLessThanOne = false)
        {
            Console.WriteLine(question);
            var userInput = Console.ReadLine();

            if (float.TryParse(userInput, out float number) && Extensions.IsPositiveAndLessThanOne(number, shouldBeLessThanOne))
            {
                ConsoleHelpers.ClearConsole();
                numberToReturn = number;
            }
            else
            {
                ConsoleHelpers.DisplayError(userInput);
                GetVariableFromUser(question, out numberToReturn, shouldBeLessThanOne);
            }
            ConsoleHelpers.ClearConsole();
        }

        public static FileName SelectTopologyFile()
        {
            ConsoleHelpers.ClearConsole();
            while (true)
            {
                Console.WriteLine("Choose topology file. Please type in number 1,2,3");
                Console.WriteLine("1: net12_1.txt");
                Console.WriteLine("2: net12_2.txt");
                Console.WriteLine("3: net4.txt");

                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 3 || selectedNumber < 1))
                {
                    ConsoleHelpers.ClearConsole();
                    return GetFileBasedOnUserInput(selectedNumber);
                }

                ConsoleHelpers.DisplayError(userInput);
            }
        }

        private static FileName GetFileBasedOnUserInput(int number)
        {
            return number switch
            {
                1 => FileName.Net12_1,
                2 => FileName.Net12_2,
                3 => FileName.Net4,
                _ => FileName.None
            };
        }

        public static ProblemType SelectProblemToSolve()
        {
            ConsoleHelpers.ClearConsole();
            while (true)
            {
                Console.WriteLine("Choose problem type. Please type in number 1 or 2");
                Console.WriteLine("1: DDAP");
                Console.WriteLine("2: DAP");

                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 2 || selectedNumber < 1))
                {
                    ConsoleHelpers.ClearConsole();
                    return (ProblemType) selectedNumber;
                }

                ConsoleHelpers.DisplayError(userInput);
            }
        }

        public static StoppingCriteria SelectStoppingCriteria()
        {
            ConsoleHelpers.ClearConsole();
            while (true)
            {
                Console.WriteLine("Choose stopping criteria for evolutionary algorithm. Please type in number 1,2,3 or 4");
                Console.WriteLine("1: Elapsed time");
                Console.WriteLine("2: Number of generations");
                Console.WriteLine("3: Number of mutations");
                Console.WriteLine("4: No improvement since last N generations");

                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 4 || selectedNumber < 1))
                {
                    ConsoleHelpers.ClearConsole();
                    return (StoppingCriteria) selectedNumber;
                }

                ConsoleHelpers.DisplayError(userInput);
            }
        }
    }
}
