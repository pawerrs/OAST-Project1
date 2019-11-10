using OAST.Project1.Common.Enums;
using OAST.Project1.Models.Common;
using OAST.Project1.Services.Genetic;

namespace OAST.Project1.Services.Optimization
{
    public class OptimizationService
    {
        private readonly MenuOptions _menuOptions;
        public OptimizationService(MenuOptions menuOptions)
        {
            _menuOptions = menuOptions;
        }

        public void OptimizeNetwork()
        {
            if (_menuOptions.AlgorithmType == AlgorithmType.BruteForce)
            {
                //TODO: Call brute force service
            }
            else if (_menuOptions.AlgorithmType == AlgorithmType.Evolutionary)
            {
                var geneticService = new GeneticService(_menuOptions);
                geneticService.Solve();
            }
        }
    }
}
