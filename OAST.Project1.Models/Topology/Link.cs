
namespace OAST.Project1.Models.Topology
{
    public class Link
    {
        public int LinkId { get; set; }
        public int StartNode { get; set; }
        public int EndNode { get; set; }
        public int NumberOfModules { get; set; }
        public double ModuleCost { get; set; }
        public int LinkModule { get; set; }
        public int TotalLoad { get; set; }
        public int SignalsCount { get; set; }

        public Link(int id, int startNode, int endNode, int numberOfModules, int moduleCost, int linkModule)
        {
            LinkId = id;
            StartNode = startNode;
            EndNode = endNode;
            NumberOfModules = numberOfModules;
            ModuleCost = moduleCost;
            LinkModule = linkModule;
        }       
    }
}
