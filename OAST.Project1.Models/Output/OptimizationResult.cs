﻿using OAST.Project1.Models.Topology;
using System.Collections.Generic;

namespace OAST.Project1.Models.Output
{
    public class OptimizationResult
    {
        public List<Link> Links { get; set; }
        public List<Demand> Demands { get; set; }
        public double TotalCost { get; set; }

        public OptimizationResult(Network network)
        {
            Links = new List<Link>(network.Links);
            Demands = new List<Demand>(network.Demands);
        }
    }
}
