using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OAST.Project1.Models.Genetic;
using OAST.Project1.Common.Enums;
using OAST.Project1.Common.Extensions;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Topology;
using OAST.Project1.Services.Helpers;

namespace OAST.Project1.Services.Genetic
{
    public class GeneticService
    {
        private readonly ProblemType _problemType;
        private readonly GeneticAlgorithmParameters _parameters;
        private readonly Network _network;

        public GeneticService(MenuOptions menuOptions)
        {
            IFileReaderService fileReaderService = new FileReaderService();
            var fileName = Extensions.GetFileName(menuOptions.FileName);
            IFileParserService fileParser = new FileParserService(fileReaderService, fileName);

            _network = fileParser.LoadTopology(fileParser.GetConfigurationLines());
            _parameters = menuOptions.GeneticAlgorithmParameters;
            _problemType = menuOptions.ProblemType;
        }
        
        public void Solve()
        {
            var state = new GeneticAlgorithmState
            {
                ElapsedTime = Stopwatch.StartNew(),
                NumberOfGenerations = 0,
                NumberOfGenerationsWithoutImprovement = 0,
                NumberOfMutations = 0,
                BestChromosome = null
            };

            _network.PossibleLinkLoads = NetworkHelper.GetPossibleDemandPathLoadSets(_network);

            var random = new Random(_parameters.RandomSeed);

            var population = GenerateInitialPopulation(random);
            state.BestChromosome = population.Chromosomes.OrderByDescending(x => x.Fitness).First();

            while (!EvaluateStoppingCriteria(state))
            {
                population.Chromosomes = EvaluateFitness(population.Chromosomes);

                var bestChromosomeInGeneration = population.Chromosomes.OrderByDescending(x => x.Fitness).First();
                if (bestChromosomeInGeneration.Fitness > state.BestChromosome.Fitness)
                {
                    state.BestChromosome = bestChromosomeInGeneration;
                    state.NumberOfGenerationsWithoutImprovement = 0;
                }
                else
                {
                    state.NumberOfGenerationsWithoutImprovement++;
                }

                var parents = SelectParents(population, random);
                var children = CrossoverParents(parents, random);
                children = MutateChildren(children, state, random);
                children = EvaluateFitness(children);

                population = SelectSurvivors(population, children);
                state.NumberOfGenerations++;
            }
        }
        
        private List<Chromosome> EvaluateFitness(List<Chromosome> chromosomes)
        {
            throw new NotImplementedException();
        }

        private List<Chromosome> SelectParents(Population population, Random random)
        {
            var parents = new List<Chromosome>();

            var sum = population.Chromosomes.Sum(x => x.Fitness);

            for (var i = 0; i < 2; i++)
            {
                var r = random.Next(sum);

                var currentSum = 0;
                foreach (var chromosome in population.Chromosomes)
                {
                    currentSum += chromosome.Fitness;

                    if (currentSum <= r)
                    {
                        continue;
                    }

                    parents.Add(chromosome);
                    break;
                }
            }

            return parents;
        }

        private List<Chromosome> MutateChildren(List<Chromosome> children, GeneticAlgorithmState state, Random random)
        {
            var child1 = children[0];
            var child2 = children[1];

            var rand = random.NextDouble();
            if (rand >= _parameters.MutationProbability)
            {
                state.NumberOfMutations++;

                child1.Mutate(random);
            }

            rand = random.NextDouble();
            if (rand >= _parameters.MutationProbability)
            {
                state.NumberOfMutations++;

                child2.Mutate(random);
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
            var newGeneration = population.Chromosomes.OrderByDescending(x => x.Fitness).Take(8);
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
