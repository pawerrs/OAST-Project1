using System;
using OAST.Project1.Common.Enums;

namespace OAST.Project1
{
    public static class Menu
    {
        public static void DisplayMenu()
        {
            ConsoleHelpers.SetConsoleColor(ConsoleColor.DarkCyan);

            // algorithm type
            AlgoritmType algorithmType = UserInteractions.GetAlgorithmType();

            //input parameters
            UserInteractions.GetVariableFromUser("Please, set population size (chromosomes count): ", out float populationSize);
            UserInteractions.GetVariableFromUser("Please, set crossover  probability <0,1>: ", out float crossoverProbability, true);
            UserInteractions.GetVariableFromUser("Please, set mutation probability <0,1>: ", out float mutationProbability, true);
            UserInteractions.GetVariableFromUser("Please, set maximal number of seconds for algorithm to work: ",
                out float maxTimeInSeconds);
            UserInteractions.GetVariableFromUser("Please, set maximal number of generations for algorithm to work: ",
                out float maxNumberOfGenerations);
            UserInteractions.GetVariableFromUser("Please, type in a random number: ", out float seed);

            // file with topology name
            FileName fileName = UserInteractions.SelectTopologyFile();

            // DAP or DDAP
            ProblemType problemType = UserInteractions.SelectProblemToSolve();
        }
    }
}
