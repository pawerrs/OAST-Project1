using System.Collections.Generic;

namespace OAST.Project1.Models.Genetic
{
    public class Population
    {
        public Population(List<Chromosome> chromosomes)
        {
            Chromosomes = chromosomes;
        }

        public List<Chromosome> Chromosomes { get; set; }
    }
}
