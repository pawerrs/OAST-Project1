using System.Diagnostics;
using OAST.Project1.Models.Output;

namespace OAST.Project1.Models.Genetic
{
    public class GeneticAlgorithmState
    {
        public Stopwatch ElapsedTime { get; set; }

        public int NumberOfGenerations { get; set; }

        public int NumberOfMutations { get; set; }

        public int NumberOfGenerationsWithoutImprovement { get; set; }

        public OptimizationResult BestChromosomeOptimizationResult { get; set; }

        public int BestChromosomeFitness { get; set; }
    }
}
