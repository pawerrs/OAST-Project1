using System.Collections.Generic;

namespace OAST.Project1.Models.Topology
{
    public class PossibleDemandPathLoadSet
    {
        public Demand Demand { get; set; }

        public Dictionary<int, List<DemandPathLoad>> PossibleDemandPathLoads { get; set; }
    }
}
