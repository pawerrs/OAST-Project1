using OAST.Project1.Models.Output;
using OAST.Project1.Models.Topology;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAST.Project1.Services.Helpers
{
    public class DDAPCostCalculator
    {
        public OptimizationResult CalculateDDAPCost(Network network)
        {
            OptimizationResult result = new OptimizationResult(network);

            FillLinksWithLoad(result);
            CalculateLinksCost(result);

            return result;
        }

        private void CalculateLinksCost(OptimizationResult result)
        {
            foreach(Link link in result.Links)
            {
                result.TotalCost += GetLinkCost(link);
            }
        }

        private double GetLinkCost(Link link)
        {
            return Math.Ceiling(Convert.ToDouble(link.TotalLoad) / Convert.ToDouble(link.LinkModule)) * link.ModuleCost;
        }

        private void FillLinksWithLoad(OptimizationResult result)
        {
            foreach (Demand demand in result.Demands)
            {
                foreach (DemandPath demandPath in demand.DemandPaths)
                {
                    //result.Links.FindAll(x => demandPath.LinkList.)
                    for (int i = 0; i < demandPath.LinkList.Length; i++)
                    {
                        if (demandPath.Load > 0)
                        {
                            Link tempLink = result.Links.Find(x => x.LinkId == demandPath.LinkList[i]);
                            tempLink.TotalLoad += demandPath.Load;
                            tempLink.SignalsCount++;
                        }
                    }
                }
            }
        }
    }
}
