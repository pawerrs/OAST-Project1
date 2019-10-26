using Microsoft.AspNetCore.Mvc;
using OAST.Project1.Models.Genetic;
using OAST.Project1.Services.Genetic;
using System.Threading.Tasks;

namespace OAST.Project1.Controllers
{
    [Route("api/genetic")]
    public class GeneticController : Controller
    {
        public IGeneticService _geneticService;

        public GeneticController (IGeneticService geneticService)
        {
            _geneticService = geneticService;
        }

        [HttpPost]
        public async Task Solve([FromBody] GeneticAlgorithmParameters parameters)
        {
            await _geneticService.SolveAllocationProblem(parameters);
        }
    }
}
