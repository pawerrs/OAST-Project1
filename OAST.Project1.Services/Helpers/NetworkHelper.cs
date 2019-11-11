using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.Services.Helpers
{
    public static class NetworkHelper
    {
        public static int CalculateLinkSize(int linkLoad, int linkModule)
        {
            if (linkModule == 0)
            {
                throw new ArgumentException("Link module can't be 0");
            }

            return (linkLoad + linkModule - 1) / linkModule;
        }

        public static int CalculateCostOfLinks(List<int> linkSizes, int linkCost)
        {
            return linkSizes.Sum(x => x * linkCost);
        }

        public static IEnumerable<PossibleDemandPathLoadSet> GetPossibleDemandPathLoadSets(Network network)
        {
            return network.Demands
                .Select(demand => new PossibleDemandPathLoadSet {Demand = demand, PossibleDemandPathLoads = GetDemandPathLoadsForDemand(demand)})
                .ToList();
        }

        private static Dictionary<int, List<DemandPathLoad>> GetDemandPathLoadsForDemand(Demand demand)
        {
            var possibleDemandPathLoads = new Dictionary<int, List<DemandPathLoad>>();
            var demandPaths = demand.DemandPaths.ToList();
            var allDemandPathLoads = GetCombinationsWithRepetition(new List<DemandPath>(demandPaths), demand.Volume);

            for (var i = 0; i < allDemandPathLoads.Count; i++)
            {
                var demandPathLoads = new List<DemandPathLoad>();

                foreach (var demandPath in demandPaths)
                {
                    demandPathLoads.Add(new DemandPathLoad
                    {
                        DemandPath = demandPath,
                        Load = allDemandPathLoads[i].Count(x => x.Id == demandPath.Id)
                    });
                }
                possibleDemandPathLoads.Add(i, demandPathLoads);
            }

            return possibleDemandPathLoads;
        }

        private static List<List<DemandPath>> GetCombinationsWithRepetition(List<DemandPath> demandPaths, int demandVolume)
        {
            var combinations = new List<List<DemandPath>>();

            if (demandVolume== 0)
            {
                var emptyCombination = new List<DemandPath>();
                combinations.Add(emptyCombination);

                return combinations;
            }

            if (demandPaths.Count == 0)
            {
                return combinations;
            }

            var head = demandPaths[0];
            var copiedCombinationList = new List<DemandPath>(demandPaths);

            var subCombinations = GetCombinationsWithRepetition(copiedCombinationList, demandVolume - 1);

            foreach (var subCombination in subCombinations)
            {
                subCombination.Insert(0, head);
                combinations.Add(subCombination);
            }

            demandPaths.RemoveAt(0);
            combinations.AddRange(GetCombinationsWithRepetition(demandPaths, demandVolume));

            return combinations;
        }
    }
}
