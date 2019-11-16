using System.Collections.Generic;

namespace OAST.Project1.Models.Topology
{
    public class FlowAllocation
    {
        public FlowAllocation(Demand demand)
        {
            Demand = demand;
        }

        public FlowAllocation(Demand demand, List<DemandPathLoad> newDemandPathLoads) : this(demand)
        {
            DemandPathLoads = newDemandPathLoads;
        }

        public Demand Demand { get; set; }

        public List<DemandPathLoad> DemandPathLoads { get; set; }
    }
}
