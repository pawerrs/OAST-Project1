
namespace OAST.Project1.Models.Topology
{
    public class DemandPath
    {
        public int Id { get; set; }
        public int Load { get; set; }
        public int[] LinkList { get; set; }

        public DemandPath(int id, int[] linkList)
        {
            Id = id;
            LinkList = linkList;
        }
    }
}
