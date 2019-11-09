
namespace OAST.Project1.Models.Topology
{
    public class DemandPath
    {
        public int Id { get; set; }
        public int[] Paths { get; set; }

        public DemandPath(int id, int[] paths)
        {
            Id = id;
            Paths = paths;
        }
    }
}
