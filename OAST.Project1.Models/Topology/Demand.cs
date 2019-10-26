using System.Collections.Generic;

namespace OAST.Project1.Models.Topology
{
    public class Demand
    {
        public int Id { get; set; }
        public int StartNode { get; set; }
        public int EndNode { get; set; }
        public int Volume { get; set; }
        public int NumberOfDemandPaths { get; set; }
        public IEnumerable<DemandPath> DemandPaths { get; set; }
    }
}
