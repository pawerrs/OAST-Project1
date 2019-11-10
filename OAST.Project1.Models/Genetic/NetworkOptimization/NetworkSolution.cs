using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.Models.Genetic.NetworkOptimization
{
    public class NetworkSolution
    {
        public List<FlowAllocation> Genes { get; set; }

        public NetworkSolution(Network network, Random random)
        {
            for (var i = 0; i < network.NumberOfDemands; i++)
            {
                var flowAllocation = new FlowAllocation(network.Demands.Single(x => x.Id == i));
                var possibleLinkLoads = flowAllocation.GetPossibleLinkLoads();

                var randomLinkLoadId = random.Next(0, possibleLinkLoads.Count - 1);
                flowAllocation.LinkLoads = possibleLinkLoads[randomLinkLoadId];

                Genes.Add(flowAllocation);
            }
        }
    }
}