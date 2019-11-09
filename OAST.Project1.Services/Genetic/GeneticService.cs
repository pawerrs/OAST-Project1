using OAST.Project1.Models.Genetic;
using System.Threading.Tasks;
using OAST.Project1.DataAccess.ConfigHandlers;

namespace OAST.Project1.Services.Genetic
{
    public class GeneticService : IGeneticService
    {
        private readonly FileParser _fileParser;

        public GeneticService()
        {
            _fileParser = new FileParser();
        }

        public async Task SolveDAP(GeneticAlgorithmParameters parameters)
        {
            var network = _fileParser.LoadTopology(FileReader.ReadFile(@"P:\STUFF\studia\mgr\sem2\OAST\OAST.Project1.DataAccess\Input Data\net12_1"));

            
        }

        public async Task SolveDDAP(GeneticAlgorithmParameters parameters)
        {
            var network = _fileParser.LoadTopology(FileReader.ReadFile(@"P:\STUFF\studia\mgr\sem2\OAST\OAST.Project1.DataAccess\Input Data\net12_1"));

            

        }

        private void GenerateInitialPopulation()
        {

        }
    }
}
