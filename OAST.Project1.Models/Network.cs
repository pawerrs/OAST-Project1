using System.Collections.Generic;

namespace OAST.Project1.Models
{
    public class Network
    {
        public int NumberOfLinks { get; set; } = 0;
        public IEnumerable<Link> Links { get; set; }
        public int NumberOfDemands { get; set; } = 0;
        public IEnumerable<Demand> Demands { get; set; }
    }
}
