using System;
using OAST.Project1.Common.Enums;
using OAST.Project1.Helpers;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Genetic;

namespace OAST.Project1
{
    public static class Menu
    {
        public static MenuOptions DisplayMenu()
        {
            ConsoleHelpers.SetConsoleColor(ConsoleColor.DarkCyan);

            GeneticAlgorithmParameters geneticAlgorithmParameters = null;

            AlgorithmType algorithmType = UserInteractions.GetAlgorithmType();

            if (algorithmType == AlgorithmType.Evolutionary)
            {
                geneticAlgorithmParameters = GetEvolutionaryAlgorithmParameters();
            }
            
            FileName fileName = UserInteractions.SelectTopologyFile();

            ProblemType problemType = UserInteractions.SelectProblemToSolve();

            var menuOptions =  new MenuOptions
            {
                AlgorithmType = algorithmType,
                FileName = fileName,
                ProblemType = problemType,
                GeneticAlgorithmParameters = geneticAlgorithmParameters
            };
            ConsoleHelpers.PrintPickedScenario(menuOptions);

            return menuOptions;
        }

        private static GeneticAlgorithmParameters GetEvolutionaryAlgorithmParameters()
        {
            UserInteractions.GetVariableFromUser("Please, set population size (chromosomes count): ", out float populationSize);
            UserInteractions.GetVariableFromUser("Please, set crossover  probability <0,1>: ", out float crossoverProbability, true);
            UserInteractions.GetVariableFromUser("Please, set mutation probability <0,1>: ", out float mutationProbability, true);
            UserInteractions.GetVariableFromUser("Please, type in a random number: ", out float seed);
            
            var stoppingCriteria = UserInteractions.SelectStoppingCriteria();
            float limitValue = 0;

            switch (stoppingCriteria)
            {
                case StoppingCriteria.ElapsedTime:
                    UserInteractions.GetVariableFromUser("Please, set maximal number of seconds for algorithm to work: ", out limitValue);
                    break;
                case StoppingCriteria.NumberOfGenerations:
                    UserInteractions.GetVariableFromUser("Please, set maximal number of generations: ", out limitValue);
                    break;
                case StoppingCriteria.NumberOfMutations:
                    UserInteractions.GetVariableFromUser("Please, set maximal number of mutations: ", out limitValue);
                    break;
                case StoppingCriteria.NoImprovement:
                    UserInteractions.GetVariableFromUser("Please, set maximal number of generations with no improvement: ", out limitValue);
                    break;
            }
            ConsoleHelpers.ClearConsole();
            return new GeneticAlgorithmParameters
            {
                InitialPopulationSize = (int)populationSize,
                MutationProbability = mutationProbability,
                CrossoverProbability = crossoverProbability,
                RandomSeed = (int)seed,
                StoppingCriteria = stoppingCriteria,
                LimitValue = (int)limitValue
            };
        }
    }
}
