using OAST.Project1.Models.Output;
using OAST.Project1.Models.Topology;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAST.Project1.Services.Helpers
{
    public class CostCalculator
    {
        public OptimizationResult CalculateDDAPCost(Network network)
        {
            OptimizationResult result = new OptimizationResult(network);

            FillLinksWithLoad(result);
            DDAPCalculateLinksCost(result);
            return result;
        }

        public OptimizationResult CalculateDAPCost(Network network)
        {
            OptimizationResult result = new OptimizationResult(network);

            FillLinksWithLoad(result);
            DAPCalculateLinksCost(result);
            return result;
        }

        private void DDAPCalculateLinksCost(OptimizationResult result)
        {
            foreach(Link link in result.Links)
            {
                result.TotalCost += GetLinkModulesNumber(link) * link.ModuleCost;
            }
        }

        private void DAPCalculateLinksCost(OptimizationResult result)
        {
            var linkOverloads = new List<double>();
            foreach (Link link in result.Links)
            {
                var linkModules = GetLinkModulesNumber(link);
                var overload = Math.Max(0, linkModules - link.NumberOfModules);

                linkOverloads.Add(overload);
            }

            result.TotalCost = linkOverloads.Max();
        }

        private double GetLinkModulesNumber(Link link)
        {
            return Math.Ceiling(Convert.ToDouble(link.TotalLoad) / Convert.ToDouble(link.LinkModule));
        }

        private void FillLinksWithLoad(OptimizationResult result)
        {
            foreach (Demand demand in result.Demands)
            {
                foreach (DemandPath demandPath in demand.DemandPaths)
                {
                    //result.Links.FindAll(x => demandPath.LinkList.)
                    foreach (var t in demandPath.LinkList)
                    {
                        if (demandPath.Load > 0)
                        {
                            Link tempLink = result.Links.Find(x => x.LinkId == t);
                            tempLink.TotalLoad += demandPath.Load;
                            tempLink.SignalsCount++;
                        }
                    }
                }
            }
        }
    }
}
