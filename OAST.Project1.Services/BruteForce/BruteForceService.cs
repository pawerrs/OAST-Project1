using OAST.Project1.Common.Enums;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Common;
using OAST.Project1.Models.Output;
using OAST.Project1.Models.Topology;
using OAST.Project1.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAST.Project1.Services.BruteForce
{
    public class BruteForceService : IBruteForceService
    {
        private readonly Network _network;
        private FileParserService _fileParser; 
        private List<DemandDistributions> _allDistributions = new List<DemandDistributions>();
        private DDAPCostCalculator calculator;
        private OptimizationResult bestOptimizationResult;
        public BruteForceService(MenuOptions menuOptions)
        {
            IFileReaderService fileReaderService = new FileReaderService();
            var fileName = fileReaderService.GetFileName(menuOptions.FileName);

            _fileParser = new FileParserService(fileReaderService, fileName);
            _network = _fileParser.LoadTopology(_fileParser.GetConfigurationLines());
            calculator = new DDAPCostCalculator();
            bestOptimizationResult = new OptimizationResult(_network);
            bestOptimizationResult.TotalCost = Double.MaxValue;
        }

        public async Task SolveDAP()
        {
            throw new System.NotImplementedException();
        }

        public async Task SolveDDAP()
        {
            throw new System.NotImplementedException();
        }

        public void OptimizeNetwork()
        {
            Console.WriteLine("Enumerating combinations...");
            foreach (Demand demand in _network.Demands)
            {
                DemandDistributions demandDistributions = new DemandDistributions(demand.Id);
                demandDistributions.FindAllDistributions(demand.Volume, demand.NumberOfDemandPaths);
                _allDistributions.Add(demandDistributions);
            }

            Console.WriteLine("All possible combinations enumerated.");
            ShowNumberOfCombinations();
            FindCheapestPath();
            Console.WriteLine("All possible combinations checked. BruteForce algorythm has finished.");
        }

        private void ShowNumberOfCombinations()
        {
            int numberOfCombinations = 1;
            foreach(DemandDistributions demandDistributions in _allDistributions)
            {
                numberOfCombinations *= demandDistributions.distributions.Count();
            }
            Console.WriteLine("Please wait where {0} combinations are checked to find cheapest one...", numberOfCombinations);
        }

        private void FindCheapestPath()
        {

            int[] chosenDemandDistribution = new int[_allDistributions.Count()];
            for (int i=0; i<chosenDemandDistribution.Length; i++)
            {
                chosenDemandDistribution[i] = 0;
            }

            bool finishFlag = true;
            while (finishFlag)
            {
                CountNetworkCost(chosenDemandDistribution);
                ChangePathCombination(ref finishFlag, chosenDemandDistribution, 0);
            }

        }

        private void CountNetworkCost(int[] chosenDemandDistribution)
        {
            Network networkToCalculate = _network.Clone();
            for (int demandIndex = 0; demandIndex < chosenDemandDistribution.Length; demandIndex++)
            {
                for (int pathIndex = 0; pathIndex < networkToCalculate.Demands[demandIndex].DemandPaths.Count(); pathIndex++)
                {
                    networkToCalculate.Demands[demandIndex].DemandPaths[pathIndex].Load = _allDistributions[demandIndex].distributions[chosenDemandDistribution[demandIndex]][pathIndex];
                }
            }
            OptimizationResult calculationResult = calculator.CalculateDDAPCost(networkToCalculate);
            if (bestOptimizationResult.TotalCost > calculationResult.TotalCost)
            {
                bestOptimizationResult = calculationResult;
                Console.WriteLine("Network cost reduced to {0}", bestOptimizationResult.TotalCost);
            }
        }

        private void ChangePathCombination(ref bool finishFlag, int[] chosenDemandDistributions, int position)
        {
            if(chosenDemandDistributions[position] + 1 < _allDistributions[position].distributions.Count())
            {
                chosenDemandDistributions[position]++;
            }
            else
            {
                if (position == _allDistributions.Count() - 1)
                {
                    finishFlag = false;
                    return;
                }
                else
                {
                    chosenDemandDistributions[position] = 0;
                    position++;
                    ChangePathCombination(ref finishFlag, chosenDemandDistributions, position);
                }
            }
        }

    }
}
