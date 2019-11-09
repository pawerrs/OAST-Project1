
namespace OAST.Project1.Models.Topology
{
    public class Link
    { 
        public int StartNode { get; set; }
        public int EndNode { get; set; }
        public int NumberOfModules { get; set; }
        public double ModuleCost { get; set; }
        public int CapacityPerModule { get; set; } 
        public int TotalLoad { get; set; }
        public int SignalsCount { get; set; }
    }
}
