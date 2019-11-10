using OAST.Project1.Models.Topology;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAST.Project1.Models.Output
{
    public class OptimizationResult
    {
        public List<Link> Links { get; set; }
        public List<Demand> Demands { get; set; }
    }
}
