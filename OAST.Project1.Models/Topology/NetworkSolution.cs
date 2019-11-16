using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Models.Genetic;

namespace OAST.Project1.Models.Topology
{
    public class NetworkSolution : Chromosome
    {
        public NetworkSolution()
        {
        }

        public NetworkSolution(Network network, Random random)
        {
            FlowAllocations = new List<FlowAllocation>();
            PossibleDemandPathLoads = new Dictionary<Demand, Dictionary<int, List<DemandPathLoad>>>();

            for (var i = 1; i < network.NumberOfDemands + 1; i++)
            {
                var demand = network.Demands.Single(x => x.Id == i);
                var flowAllocation = new FlowAllocation(demand);
                var possibleLinkLoadsForDemand = network.GetPossibleLinkLoadsForDemand(i).PossibleDemandPathLoads;

                PossibleDemandPathLoads.Add(demand, possibleLinkLoadsForDemand);

                var randomLinkLoadId = random.Next(0, possibleLinkLoadsForDemand.Count - 1);
                flowAllocation.DemandPathLoads = possibleLinkLoadsForDemand[randomLinkLoadId];

                FlowAllocations.Add(flowAllocation);
            }
        }

        public NetworkSolution(List<FlowAllocation> flowAllocations, Dictionary<Demand, Dictionary<int, List<DemandPathLoad>>> possibleDemandPathLoads)
        {
            FlowAllocations = flowAllocations;
            PossibleDemandPathLoads = possibleDemandPathLoads;
        }

        public List<FlowAllocation> FlowAllocations { get; set; }

        public Dictionary<Demand, Dictionary<int, List<DemandPathLoad>>> PossibleDemandPathLoads { get; set; }

        public override List<Chromosome> Crossover(Chromosome chromosomeToCrossWith, Random random)
        {
            var parent2 = (NetworkSolution) chromosomeToCrossWith;

            var crossoverPoint = random.Next(FlowAllocations.Count - 1);
            var firstPartLength = crossoverPoint;
            var lastPartLength = FlowAllocations.Count - crossoverPoint;

            var parent1FirstPart = FlowAllocations.GetRange(0, firstPartLength);
            var parent1LastPart = FlowAllocations.GetRange(firstPartLength, lastPartLength);

            var parent2FirstPart = parent2.FlowAllocations.GetRange(0, firstPartLength);
            var parent2LastPart = parent2.FlowAllocations.GetRange(firstPartLength, lastPartLength);

            var child1 = (Chromosome) new NetworkSolution(parent1FirstPart.Concat(parent2LastPart).ToList(), PossibleDemandPathLoads);
            var child2 = (Chromosome) new NetworkSolution(parent2FirstPart.Concat(parent1LastPart).ToList(), PossibleDemandPathLoads);

            return new List<Chromosome> {child1, child2};
        }

        public override Chromosome Mutate(Random random)
        {
            var flowAllocations = new List<FlowAllocation>(FlowAllocations);

            var geneToMutate = random.Next(1, flowAllocations.Count);

            var flowAllocationToMutate = flowAllocations.Single(x => x.Demand.Id == geneToMutate);
            flowAllocations.Remove(flowAllocationToMutate);

            var newAllocationId = random.Next(PossibleDemandPathLoads[flowAllocationToMutate.Demand].Count - 1);
            var newDemandPathLoads = PossibleDemandPathLoads[flowAllocationToMutate.Demand][newAllocationId];
            var mutatedFlowAllocation = new FlowAllocation(flowAllocationToMutate.Demand, newDemandPathLoads);

            flowAllocations.Insert(geneToMutate - 1, mutatedFlowAllocation);

            return new NetworkSolution(flowAllocations, PossibleDemandPathLoads);
        }

        public override Chromosome Clone()
        {
            return new NetworkSolution
            {
                Fitness = Fitness,
                FlowAllocations = new List<FlowAllocation>(FlowAllocations),
                PossibleDemandPathLoads = new Dictionary<Demand, Dictionary<int, List<DemandPathLoad>>>(PossibleDemandPathLoads)
            };
        }
    }
}