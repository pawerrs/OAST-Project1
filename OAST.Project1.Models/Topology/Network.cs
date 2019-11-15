using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Models.Topology
{
    public class Network
    {
        public int NumberOfLinks { get; set; }
        public List<Link> Links { get; set; }
        public int NumberOfDemands { get; set; }
        public List<Demand> Demands { get; set; }
        public List<PossibleDemandPathLoadSet> PossibleLinkLoads { get; set; }

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
                Links = CloneLinkList(Links),
                Demands = new List<Demand>(Demands),
                PossibleLinkLoads = new List<PossibleDemandPathLoadSet>()
            };
        }

        private List<Link> CloneLinkList(List<Link> links)
        {
            foreach(Link link in links)
            {
                link.TotalLoad = 0;
                link.SignalsCount = 0;
            }
            return links;
        }
    }
}
