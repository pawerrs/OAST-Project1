using System.Collections.Generic;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.DataAccess.FileParser
{
    public interface IFileParserService
    {
        Network LoadTopology(List<string> fileLines);
        int GetNumberOfLinks();
        IEnumerable<Link> LoadAllLinks();
        Link GetOneLink(string line, ref int id);
        int GetNumberOfDemands();
        IEnumerable<Demand> LoadAllDemands();
        List<string> GetDemandSection();
        Demand GetOneDemand(List<string> demandSection);
        IEnumerable<DemandPath> GetDemandPath(IEnumerable<string> lines);
        List<string> GetConfigurationLines();
        void SetCurrentLineNumber(int number);
        void SetNumberOfDemands(int number);

    }
}
