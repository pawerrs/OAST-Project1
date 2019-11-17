using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OAST.Project1.Models.Genetic;
using OAST.Project1.Common.Enums;
using OAST.Project1.Common.Extensions;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.DataAccess.OutputWriter;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Output;
using OAST.Project1.Models.Topology;
using OAST.Project1.Services.Helpers;

namespace OAST.Project1.Services.Genetic
{
    public class GeneticService
    {
        private readonly ProblemType _problemType;
        private readonly GeneticAlgorithmParameters _parameters;
        private readonly CostCalculator _calculator;
        private readonly OutputWriter _outputWriter;
        private readonly Network _network;
        private readonly MenuOptions _menuOptions;

        public GeneticService(MenuOptions menuOptions)
        {
            IFileReaderService fileReaderService = new FileReaderService();
            var fileName = Extensions.GetFileName(menuOptions.FileName);
            IFileParserService fileParser = new FileParserService(fileReaderService, fileName);

            _network = fileParser.LoadTopology(fileParser.GetConfigurationLines());
            _parameters = menuOptions.GeneticAlgorithmParameters;
            _problemType = menuOptions.ProblemType;
            _menuOptions = menuOptions;
            _calculator = new CostCalculator();
            _outputWriter = new OutputWriter();
        }

        public void Solve()
        {
            var state = new GeneticAlgorithmState
            {
                ElapsedTime = Stopwatch.StartNew(),
                NumberOfGenerations = 0,
                NumberOfGenerationsWithoutImprovement = 0,
                NumberOfMutations = 0,
                BestChromosomeOptimizationResult = null,
                BestChromosomeFitness = 0
            };

            _network.PossibleLinkLoads = NetworkHelper.GetPossibleDemandPathLoadSets(_network);

            var random = new Random(_parameters.RandomSeed);

            var population = GenerateInitialPopulation(random);
            EvaluateFitness(population.Chromosomes);

            var bestChromosome = population.Chromosomes.OrderByDescending(x => x.Fitness).First();
            state.BestChromosomeOptimizationResult = CalculateNetworkSolutionOptimizationResult((NetworkSolution)bestChromosome);
            state.BestChromosomeFitness = (int) (1000000 / state.BestChromosomeOptimizationResult.TotalCost);

            PrintBestAlgorithmInGeneration(state, true);

            while (!EvaluateStoppingCriteria(state))
            {
                EvaluateFitness(population.Chromosomes);

                var bestChromosomeInGeneration = population.Chromosomes.OrderByDescending(x => x.Fitness).First();

                if (bestChromosomeInGeneration.Fitness > state.BestChromosomeFitness)
                {
                    state.BestChromosomeOptimizationResult = CalculateNetworkSolutionOptimizationResult((NetworkSolution)bestChromosomeInGeneration);
                    state.BestChromosomeFitness = (int)(1000000 / state.BestChromosomeOptimizationResult.TotalCost);

                    state.NumberOfGenerationsWithoutImprovement = 0;

                    PrintBestAlgorithmInGeneration(state, true);
                }
                else
                {
                    state.NumberOfGenerationsWithoutImprovement++;

                    PrintBestAlgorithmInGeneration(state, false);
                }

                var eliteOffsprings = SelectEliteOffsprings(population);
                var crossoveredOffsprings = CrossoverOffsprings(population, eliteOffsprings.Count, random);
                var mutatedOffsprings = MutateOffsprings(crossoveredOffsprings, state, random);

                EvaluateFitness(mutatedOffsprings);

                population = SelectSurvivors(population, eliteOffsprings, mutatedOffsprings);

                state.NumberOfGenerations++;
            }

            state.ElapsedTime.Stop();

            DoPostSolveActivities(state);
        }

        private List<Chromosome> MutateOffsprings(List<Chromosome> crossoveredOffsprings, GeneticAlgorithmState state, Random random)
        {
            var mutatedOffsprings = new List<Chromosome>();

            foreach (var offspring in crossoveredOffsprings)
            {
                var rand = random.NextDouble();
                if (rand >= _parameters.MutationProbability)
                {
                    state.NumberOfMutations++;

                    mutatedOffsprings.Add(offspring.Mutate(random));
                }
                else
                {
                    mutatedOffsprings.Add(offspring);
                }
            }

            return mutatedOffsprings;
        }

        private List<Chromosome> CrossoverOffsprings(Population population, int eliteOffspringsCount, Random random)
        {
            var crossoveredOffsprings = new List<Chromosome>();

            var eliteCount = Math.Max(eliteOffspringsCount, 1);
            var nonEliteOffspringsCount = population.Chromosomes.Count - eliteCount;

            for (var i = 0; i < nonEliteOffspringsCount / 2; i++)
            {
                var offspring1 = population.Chromosomes[i];
                var offspring2 = population.Chromosomes[nonEliteOffspringsCount - 1 - i];

                var rand = random.NextDouble();
                if (rand >= _parameters.CrossoverProbability)
                {
                    var crossoveredPair = offspring1.Crossover(offspring2, random);
                    crossoveredOffsprings.AddRange(crossoveredPair);
                }
                else
                {
                    crossoveredOffsprings.Add(offspring1);
                    crossoveredOffsprings.Add(offspring2);
                }
            }

            if (nonEliteOffspringsCount % 2 != 0)
            {
                crossoveredOffsprings.Add(population.Chromosomes[nonEliteOffspringsCount / 2]);
            }

            return crossoveredOffsprings;
        }

        private List<Chromosome> SelectEliteOffsprings(Population population)
        {
            var elitePercentage = 0.1;
            var eliteOffspringsCount = (int) Math.Max(elitePercentage * population.Chromosomes.Count, 1);

            return new List<Chromosome>(population.Chromosomes.OrderByDescending(x => x.Fitness).Take(eliteOffspringsCount));
        }

        private void DoPostSolveActivities(GeneticAlgorithmState state)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Best solution found: {state.BestChromosomeOptimizationResult.TotalCost}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine($"Genetic Algorithm exited after {(double)state.ElapsedTime.ElapsedMilliseconds / 1000} seconds. " +
                              $"Generations cultured: {state.NumberOfGenerations}.");

            _outputWriter.SaveOutputToTheFile(state.BestChromosomeOptimizationResult, _menuOptions);
        }

        private void PrintBestAlgorithmInGeneration(GeneticAlgorithmState state, bool bestResult)
        {
            if (bestResult)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine($"Best solution in generation { state.NumberOfGenerations }: " + state.BestChromosomeOptimizationResult.TotalCost);

            if (bestResult)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
        }

        private void EvaluateFitness(List<Chromosome> chromosomes)
        {
            foreach (var chromosome in chromosomes)
            {
                var networkSolution = (NetworkSolution)chromosome;
                var calculationResult = CalculateNetworkSolutionOptimizationResult(networkSolution);

                if (calculationResult.TotalCost == 0)
                {
                    networkSolution.Fitness = int.MaxValue;
                }
                else
                {
                    networkSolution.Fitness = (int)(1000000 / calculationResult.TotalCost);
                }
            }
        }

        private Population SelectSurvivors(Population population, List<Chromosome> eliteOffsprings, List<Chromosome> newOffsprings)
        {
            var remainingOffspringsCount = population.Chromosomes.Count - eliteOffsprings.Count;

            var weakestChromosomes = population.Chromosomes.OrderBy(x => x.Fitness).Take(remainingOffspringsCount).ToList();
            var candidates = weakestChromosomes.Concat(newOffsprings);

            var newGeneration = candidates.OrderByDescending(x => x.Fitness).Take(remainingOffspringsCount).ToList();
            newGeneration.AddRange(eliteOffsprings);

            return new Population(newGeneration);
        }

        private Population GenerateInitialPopulation(Random random)
        {
            var chromosomes = new List<Chromosome>();

            for (var i = 0; i < _parameters.InitialPopulationSize; i++)
            {
                chromosomes.Add(CreateRandomChromosome(random));
            }

            return new Population(chromosomes);
        }

        private Chromosome CreateRandomChromosome(Random random)
        {
            return new NetworkSolution(_network, random);
        }

        private OptimizationResult CalculateNetworkSolutionOptimizationResult(NetworkSolution networkSolution)
        {
            var network = _network.Clone();
            foreach (var link in network.Links)
            {
                var totalLinkLoad = 0;
                foreach (var flowAllocation in networkSolution.FlowAllocations)
                {
                    var linkLoadInDemandAllocation = flowAllocation.DemandPathLoads
                        .Where(x => x.DemandPath.LinkList.Contains(link.LinkId));

                    foreach (var demandPath in linkLoadInDemandAllocation)
                    {
                        totalLinkLoad += demandPath.Load;
                    }
                }

                link.TotalLoad = totalLinkLoad;
            }

            return _problemType == ProblemType.DDAP ? _calculator.CalculateDDAPCost(network) : _calculator.CalculateDAPCost(network);
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
