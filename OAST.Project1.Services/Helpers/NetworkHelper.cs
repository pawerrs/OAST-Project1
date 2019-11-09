using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.Services.Helpers
{
    public static class NetworkHelper
    {
        public static int CalculateLinkSize(int linkLoad, int linkModule)
        {
            if (linkModule == 0)
            {
                throw new ArgumentException("Link module can't be 0");
            }

            return (linkLoad + linkModule - 1) / linkModule;
        }

        public static int CalculateCostOfLinks(List<int> linkSizes, int linkCost)
        {
            return linkSizes.Sum(x => x * linkCost);
        }
    }
}
