using OAST.Project1.Services.Optimization;

namespace OAST.Project1
{
    class Runner
    {
        static void Main(string[] args)
        {
            var menuOptions = Menu.DisplayMenu();

            var optimizationService = new OptimizationService(menuOptions);
            optimizationService.OptimizeNetwork();
        }
    }
}
