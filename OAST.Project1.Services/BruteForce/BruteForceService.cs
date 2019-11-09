﻿using OAST.Project1.Models.Output;
using OAST.Project1.Models.Topology;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Services.BruteForce
{
    public class BruteForceService: IBruteForceService
    {
        private readonly Network _network;
        private List<DemandDistributions> _allDistributions = new List<DemandDistributions>();

        public BruteForceService(Network network)
        {
            _network = network;
        }

        public OptimizationResult OptimizeNetwork()
        {
           
            foreach (Demand demand in _network.Demands)
            {
                DemandDistributions demandDistributions = new DemandDistributions(demand.Id);
                demandDistributions.FindAllDistributions(demand.Volume, demand.NumberOfDemandPaths);
                _allDistributions.Add(demandDistributions);
            }

            OptimizationResult result = FindCheapestPath();

            return result;
        }

        private OptimizationResult FindCheapestPath()
        {
            OptimizationResult result = new OptimizationResult();
            int[] chosenDemandDistribution = new int[_allDistributions.Count()];
            for (int i=0; i<chosenDemandDistribution.Length; i++)
            {
                chosenDemandDistribution[i] = 0;
            }

            bool finishFlag = true;
            while (finishFlag)
            {
                result = CountNetworkCost(chosenDemandDistribution);
                RollChosenPaths(ref finishFlag, chosenDemandDistribution, 0);
            }

            return result;
        }

        private OptimizationResult CountNetworkCost(int[] chosenDemandDistribution)
        {
            throw new NotImplementedException();
        }

        private void RollChosenPaths(ref bool finishFlag, int[] chosenDemandDistributions, int position)
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
                    RollChosenPaths(ref finishFlag, chosenDemandDistributions, position);
                }
            }
        }

    }
}
