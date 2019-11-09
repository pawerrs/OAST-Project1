﻿using System;
using OAST.Project1.Common.Enums;
using OAST.Project1.Common.Extensions;

namespace OAST.Project1
{
    public static class UserInteractions
    {
        public static AlgoritmType GetAlgorithmType()
        {
            while (true)
            {
                ConsoleHelpers.ClearConsole();
                Console.WriteLine("Choose an algorithm");
                Console.WriteLine("1. Evolutionary Algorithm");
                Console.WriteLine("2. BruteForce Algorithm");

                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 2 || selectedNumber < 1))
                {
                    return (AlgoritmType)selectedNumber;
                }

                ConsoleHelpers.DisplayError(userInput);
            }
        }

        public static void GetVariableFromUser(string question, out float numberToReturn, bool shouldBeLessThanOne = false)
        {
            ConsoleHelpers.ClearConsole();
            Console.WriteLine(question);
            var userInput = Console.ReadLine();

            if (float.TryParse(userInput, out float number) && Extensions.IsPositiveAndLessThanOne(number, shouldBeLessThanOne))
            {
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
            Console.WriteLine("Choose topology file. Please type in number 1,2,3");
            Console.WriteLine("1: net12_1.txt");
            Console.WriteLine("1: net12_2.txt");
            Console.WriteLine("1: net4.txt");

            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 3 || selectedNumber < 1))
            {
                return GetFileBasedOnUserInput(selectedNumber);
            }
            else
            {
                ConsoleHelpers.DisplayError(userInput);
                SelectTopologyFile();
            }
            return FileName.None;
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
            Console.WriteLine("***********************************************************");
            Console.WriteLine("Choose problem type. Please type in number 1 or 2");
            Console.WriteLine("1: DDAP");
            Console.WriteLine("1: DAP");

            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out var selectedNumber) && !(selectedNumber > 2 || selectedNumber < 1))
            {
                return (ProblemType) selectedNumber;
            }
            else
            {
                ConsoleHelpers.DisplayError(userInput);
                SelectTopologyFile();
            }
            return ProblemType.None;
        }
    }
}
