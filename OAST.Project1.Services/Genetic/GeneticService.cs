using System;
using System.Collections.Generic;
using System.Diagnostics;
using OAST.Project1.Models.Genetic;
using OAST.Project1.Common.Enums;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Topology;
using OAST.Project1.Services.Helpers;

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

            OptimizeWithGeneticAlgorithm(fitnessFunction);
        }

        private void OptimizeWithGeneticAlgorithm(Action fitnessFunction)
        {
            var state = new GeneticAlgorithmState
            {
                ElapsedTime = Stopwatch.StartNew(),
                NumberOfGenerations = 0,
                NumberOfGenerationsWithoutImprovement = 0,
                NumberOfMutations = 0
            };

            _network.PossibleLinkLoads = NetworkHelper.GetPossibleDemandPathLoadSets(_network);

            var random = new Random(_parameters.RandomSeed);

            var population = GenerateInitialPopulation(random);

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

        private Population GenerateInitialPopulation(Random random)
        {
            var chromosomes = new List<Chromosome>();

            for (var i = 0; i < _parameters.InitialPopulationSize; i++)
            {
                var chromosome = CreateRandomChromosome(random);
                chromosomes.Add(CreateRandomChromosome(random));
            }

            return new Population(chromosomes);
        }

        private Chromosome CreateRandomChromosome(Random random)
        {
            return new NetworkSolution(_network, random);
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
