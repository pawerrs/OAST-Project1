using System;
using System.Collections.Generic;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.Models.Genetic.NetworkOptimization
{
    public class FlowAllocation
    {
        public FlowAllocation(Demand demand)
        {
            Demand = demand;
        }

        public Demand Demand { get; set; }

        public List<LinkLoad> LinkLoads { get; set; }

        public Dictionary<int, List<LinkLoad>> GetPossibleLinkLoads()
        {
            //TODO: Get all possible link loads for paths for the demand and assign dictionary keys

            throw new NotImplementedException();
        }
    }
}
