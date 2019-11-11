using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Models.Topology
{
    public class Network
    {
        public int NumberOfLinks { get; set; } = 0;
        public IEnumerable<Link> Links { get; set; }
        public int NumberOfDemands { get; set; } = 0;
        public IEnumerable<Demand> Demands { get; set; }
        public IEnumerable<PossibleDemandPathLoadSet> PossibleLinkLoads { get; set; }

        public PossibleDemandPathLoadSet GetPossibleLinkLoadsForDemand(int demandId)
        {
            return PossibleLinkLoads.Single(x => x.Demand.Id == demandId);
        }

        public Network Clone()
        {
            return new Network
            {
                NumberOfLinks = NumberOfLinks,
                NumberOfDemands = NumberOfDemands,
                Links = new List<Link>(Links),
                Demands = new List<Demand>(Demands),
                PossibleLinkLoads = new List<PossibleDemandPathLoadSet>()
            };
        }
    }
}
