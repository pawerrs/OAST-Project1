using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Models.Topology
{
    public class Demand
    {
        public int Id { get; set; }
        public int StartNode { get; set; }
        public int EndNode { get; set; }
        public int Volume { get; set; }
        public int NumberOfDemandPaths { get; set; }
        public List<DemandPath> DemandPaths { get; set; }

        public Demand(int id, int startNode, int endNode, int volume, int numberOfDemandPaths, List<DemandPath> demandPaths)
        {
            Id = id;
            StartNode = startNode;
            EndNode = endNode;
            Volume = volume;
            NumberOfDemandPaths = numberOfDemandPaths;
            DemandPaths = demandPaths;
        }
    }
}
