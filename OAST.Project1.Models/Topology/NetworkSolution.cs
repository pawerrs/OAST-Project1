using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Models.Genetic;

namespace OAST.Project1.Models.Topology
{
    public class NetworkSolution : Chromosome
    {
        public List<FlowAllocation> FlowAllocations { get; set; }

        public NetworkSolution(Network network, Random random)
        {
            FlowAllocations = new List<FlowAllocation>();

            for (var i = 1; i < network.NumberOfDemands + 1; i++)
            {
                var flowAllocation = new FlowAllocation(network.Demands.Single(x => x.Id == i));
                var possibleLinkLoadsForDemand = network.GetPossibleLinkLoadsForDemand(i).PossibleDemandPathLoads;

                var randomLinkLoadId = random.Next(0, possibleLinkLoadsForDemand.Count - 1);
                flowAllocation.DemandPathLoads = possibleLinkLoadsForDemand[randomLinkLoadId];

                FlowAllocations.Add(flowAllocation);
            }
        }
    }
}