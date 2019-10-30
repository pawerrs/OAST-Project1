using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OAST.Project1.Services.BruteForce;

namespace OAST.Project1.Controllers
{
    [Route("api/bruteForce")]
    public class BruteForceController : Controller
    {
        private IBruteForceService _bruteForceService;
        public BruteForceController(IBruteForceService bruteForceService)
        {
            _bruteForceService = bruteForceService;
        }

        [HttpPost]
        public async Task SolveDAP()
        {
            await _bruteForceService.SolveDAP();
        }
    }
}
