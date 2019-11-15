using OAST.Project1.Common.Enums;
using OAST.Project1.Models.Genetic;

namespace OAST.Project1.Models.Common
{
    public class MenuOptions
    {
        public AlgorithmType AlgorithmType { get; set; }

        public FileName FileName { get; set; }

        public ProblemType ProblemType { get; set; }

        public GeneticAlgorithmParameters GeneticAlgorithmParameters { get; set; }
    }
}
