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
using OAST.Project1.Common.Extensions;
using OAST.Project1.DataAccess.OutputWriter;

namespace OAST.Project1.Services.BruteForce
{
    public class BruteForceService : IBruteForceService
    {
        private readonly Network _network;
        private readonly List<DemandDistributions> _allDistributions = new List<DemandDistributions>();
        private readonly CostCalculator _calculator;
        private OptimizationResult _bestOptimizationResult;
        private readonly MenuOptions _menuOptions;
        public BruteForceService(MenuOptions menuOptions)
        {
            _menuOptions = menuOptions;
            IFileReaderService fileReaderService = new FileReaderService();
            var fileName = Extensions.GetFileName(_menuOptions.FileName);
            var fileParser = new FileParserService(fileReaderService, fileName);
            _network = fileParser.LoadTopology(fileParser.GetConfigurationLines());
            _calculator = new CostCalculator();
            _bestOptimizationResult = new OptimizationResult(_network) {TotalCost = double.MaxValue};
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
            FindCheapestPath(_menuOptions.ProblemType);
            Console.WriteLine("All possible combinations checked. BruteForce algorithm has finished.");

            new OutputWriter().SaveOutputToTheFile(_bestOptimizationResult, _menuOptions);
        }

        private void ShowNumberOfCombinations()
        {
            double magnitudeOfCombinations = _allDistributions.Sum(demandDistributions => Math.Log10(demandDistributions.distributions.Count()));
            Console.WriteLine("Please wait while approximately 10e{0} combinations are checked to find the cheapest one...", magnitudeOfCombinations);
        }

        private void FindCheapestPath(ProblemType problemType)
        {

            int[] chosenDemandDistribution = new int[_allDistributions.Count()];
            for (int i=0; i<chosenDemandDistribution.Length; i++)
            {
                chosenDemandDistribution[i] = 0;
            }

            bool finishFlag = true;
            while (finishFlag)
            {
                CountNetworkCost(chosenDemandDistribution, problemType);
                ChangePathCombination(ref finishFlag, chosenDemandDistribution, 0);
            }

        }

        private void CountNetworkCost(int[] chosenDemandDistribution, ProblemType problemType)
        {
            Network networkToCalculate = _network.Clone();
            for (int demandIndex = 0; demandIndex < chosenDemandDistribution.Length; demandIndex++)
            {
                for (int pathIndex = 0; pathIndex < networkToCalculate.Demands[demandIndex].DemandPaths.Count(); pathIndex++)
                {
                    networkToCalculate.Demands[demandIndex].DemandPaths[pathIndex].Load = _allDistributions[demandIndex].distributions[chosenDemandDistribution[demandIndex]][pathIndex];
                }
            }
            OptimizationResult calculationResult = problemType == ProblemType.DDAP ? _calculator.CalculateDDAPCost(networkToCalculate) : _calculator.CalculateDAPCost(networkToCalculate);
            if (_bestOptimizationResult.TotalCost > calculationResult.TotalCost)
            {
                _bestOptimizationResult = calculationResult;
                Console.WriteLine("Network cost reduced to {0}", _bestOptimizationResult.TotalCost);
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
