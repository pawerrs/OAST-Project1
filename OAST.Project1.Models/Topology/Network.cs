using System.Collections.Generic;

namespace OAST.Project1.Models.Topology
{
    public class Network
    {
        public int NumberOfLinks { get; set; } = 0;
        public List<Link> Links { get; set; }
        public int NumberOfDemands { get; set; } = 0;
        public List<Demand> Demands { get; set; }
    }
}
