using System.Diagnostics;

namespace OAST.Project1.Models.Genetic
{
    public class GeneticAlgorithmState
    {
        public Stopwatch ElapsedTime { get; set; }

        public int NumberOfGenerations { get; set; }

        public int NumberOfMutations { get; set; }

        public int NumberOfGenerationsWithoutImprovement { get; set; }
    }
}
