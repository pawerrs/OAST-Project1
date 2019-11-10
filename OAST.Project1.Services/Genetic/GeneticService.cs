using System;
using System.Collections.Generic;
using System.Diagnostics;
using OAST.Project1.Models.Genetic;
using OAST.Project1.Common.Enums;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Genetic.NetworkOptimization;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.Services.Genetic
{
    public class GeneticService
    {
        private readonly IFileParserService _fileParser;
        private readonly FileName _fileName;
        private readonly ProblemType _problemType;
        private readonly GeneticAlgorithmParameters _parameters;
        private readonly Network _network;

        public GeneticService(MenuOptions menuOptions)
        {
            IFileReaderService fileReaderService = new FileReaderService();
            var fileName = fileReaderService.GetFileName(menuOptions.FileName);

            _fileParser = new FileParserService(fileReaderService, fileName);
            _network = _fileParser.LoadTopology(_fileParser.GetConfigurationLines());
            _parameters = menuOptions.GeneticAlgorithmParameters;
            _fileName = menuOptions.FileName;
            _problemType = menuOptions.ProblemType;
        }

        public void Solve()
        {
            Action fitnessFunction = null;

            switch (_problemType)
            {
                case ProblemType.DDAP:
                    fitnessFunction = EvaluateDDAPSolution;
                    break;
                case ProblemType.DAP:
                    fitnessFunction = EvaluateDAPSolution;
                    break;
            }

            OptimizeWithGeneticAlgorithm<NetworkSolution>(fitnessFunction);
        }

        private void OptimizeWithGeneticAlgorithm<T>(Action fitnessFunction)
        {
            var state = new GeneticAlgorithmState
            {
                ElapsedTime = Stopwatch.StartNew(),
                NumberOfGenerations = 0,
                NumberOfGenerationsWithoutImprovement = 0,
                NumberOfMutations = 0
            };

            var random = new Random(_parameters.RandomSeed);
            
            var population = GenerateInitialPopulation<T>();

            while (!EvaluateStoppingCriteria(state))
            {

            }
        }

        private void EvaluateDDAPSolution()
        {


        }

        private void EvaluateDAPSolution()
        {

        }

        private Population<T> GenerateInitialPopulation<T>()
        {
            var population = new Population<T>();
            population.Chromosomes = new List<T>();

            for (var i = 0; i < _parameters.InitialPopulationSize; i++)
            {

            }

            return population;
        }

        private T CreateRandomChromosome<T>() where T : new()
        {
            return new T();
        }

        private bool EvaluateStoppingCriteria(GeneticAlgorithmState state) =>
            _parameters.StoppingCriteria switch
            {
                StoppingCriteria.ElapsedTime => state.ElapsedTime.ElapsedMilliseconds / 1000 >= _parameters.LimitValue,
                StoppingCriteria.NoImprovement => state.NumberOfGenerationsWithoutImprovement >= _parameters.LimitValue,
                StoppingCriteria.NumberOfGenerations => state.NumberOfGenerations >= _parameters.LimitValue,
                StoppingCriteria.NumberOfMutations => state.NumberOfGenerations >= _parameters.LimitValue,
                _ => false
            };
    }
}
