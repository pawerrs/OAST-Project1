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

                var parents = SelectParents(population, random);
                var children = CrossoverParents(parents, random);
                var mutatedChildren = MutateChildren(children, state, random);

                EvaluateFitness(mutatedChildren);

                population = SelectSurvivors(population, mutatedChildren);

                state.NumberOfGenerations++;
            }

            DoPostSolveActivities(state);
        }

        private void DoPostSolveActivities(GeneticAlgorithmState state)
        {
            Console.WriteLine("Best solution found: " + state.BestChromosomeOptimizationResult.TotalCost);
            //_outputWriter.SaveOutputToTheFile(bestSolutionResult, _menuOptions);
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

                networkSolution.Fitness = (int)(1000000 / calculationResult.TotalCost);
            }
        }

        private List<Chromosome> SelectParents(Population population, Random random)
        {
            var generation = new List<Chromosome>(population.Chromosomes).OrderBy(x => x.Fitness).ToList();
            var parents = new List<Chromosome>();

            var sum = generation.Sum(x => x.Fitness);

            for (var i = 0; i < 2; i++)
            {
                var r = random.Next(sum);

                var currentSum = 0;
                foreach (var chromosome in generation)
                {
                    currentSum += chromosome.Fitness;

                    if (currentSum <= r)
                    {
                        continue;
                    }

                    var networkSolution = (NetworkSolution)chromosome;
                    parents.Add(new NetworkSolution(networkSolution.FlowAllocations, networkSolution.PossibleDemandPathLoads));
                    break;
                }
            }

            return parents;
        }

        private List<Chromosome> MutateChildren(List<Chromosome> children, GeneticAlgorithmState state, Random random)
        {
            var mutatedChildren = new List<Chromosome>();

            var rand = random.NextDouble();
            if (rand >= _parameters.MutationProbability)
            {
                state.NumberOfMutations++;

                mutatedChildren.Add(children[0].Mutate(random));
            }
            else
            {
                mutatedChildren.Add(children[0]);
            }

            rand = random.NextDouble();
            if (rand >= _parameters.MutationProbability)
            {
                state.NumberOfMutations++;

                mutatedChildren.Add(children[1].Mutate(random));
            }
            else
            {
                mutatedChildren.Add(children[1]);
            }

            return children;
        }

        private List<Chromosome> CrossoverParents(List<Chromosome> parents, Random random)
        {
            List<Chromosome> children;

            var rand = random.NextDouble();
            if (rand >= _parameters.CrossoverProbability)
            {
                var parent1 = parents[0];
                var parent2 = parents[1];

                children = parent1.Crossover(parent2, random);
            }
            else
            {
                children = parents;
            }

            return children;
        }

        private Population SelectSurvivors(Population population, List<Chromosome> children)
        {
            var weakestChromosomes = population.Chromosomes.OrderBy(x => x.Fitness).Take(2).ToList();
            var newGeneration = population.Chromosomes.OrderByDescending(x => x.Fitness).Take(_parameters.InitialPopulationSize - 2);
            var candidates = weakestChromosomes.Concat(children);

            newGeneration = newGeneration.Concat(candidates.OrderByDescending(x => x.Fitness).Take(2)).ToList();

            return new Population(newGeneration.ToList());
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
