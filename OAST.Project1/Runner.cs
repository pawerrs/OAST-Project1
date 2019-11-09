using OAST.Project1.Common.Enums;
using OAST.Project1.Models.Common;

namespace OAST.Project1
{
    class Runner
    {
        static void Main(string[] args)
        {
            var menuOptions = Menu.DisplayMenu();
        }

        private static void SolveGivenProblem(MenuOptions menuOptions)
        {
            if (menuOptions.AlgorithmType == AlgorithmType.BruteForce)
            {
                //TODO: Call brute force service
            }
            else if (menuOptions.AlgorithmType == AlgorithmType.Evolutionary)
            {
                //TODO: Call evolutionary service 
            }
        }
    }
}
