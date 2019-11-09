using OAST.Project1.Models.Genetic;
using System.Threading.Tasks;

namespace OAST.Project1.Services.Genetic
{
    public interface IGeneticService
    {
        Task SolveDAP(GeneticAlgorithmParameters parameters);
        Task SolveDDAP(GeneticAlgorithmParameters parameters);
    }
}