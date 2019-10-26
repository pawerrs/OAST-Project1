namespace OAST.Project1.Models.Genetic
{
    public class GeneticAlgorithmParameters
    {
        public int InitialPopulationSize { get; set; }

        public float MutationProbability { get; set; }

        public float CrossoverProbability { get; set; }

        public StoppingCriteria StoppingCriteria { get; set; }
    }
}
