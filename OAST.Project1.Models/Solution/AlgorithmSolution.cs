using OAST.Project1.Models.Topology;
using System.Collections.Generic;

namespace OAST.Project1.Models.Solution
{
    public class AlgorithmSolution
    {
        public List<int> LinkLoads { get; set; }
        public float CostResult { get; set; }
        private Dictionary<Point, int> Values { get; set; }
        public int LinksWIthExceededCapacity { get; set; }
    }
}
