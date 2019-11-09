
using System;
using System.Collections.Generic;
using System.Linq;
using OAST.Project1.Common.Extensions;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.DataAccess.ConfigHandlers
{
    public class FileParser
    {
        private readonly Network _network = new Network();
        private List<string> _fileLines;
        private const string Separator = "-1"; //TODO configuration
        private int _demandId;

        public Network LoadTopology(List<string> fileLines)
        {
            _fileLines = fileLines;
            _network.NumberOfLinks = GetNumberOfLinks();
            _network.Links = LoadAllLinks();
            _network.NumberOfDemands = GetNumberOfDemands();
            _network.Demands = LoadAllDemands();
            return _network;
        }

        private int GetNumberOfLinks()
        {
            return int.Parse(_fileLines.RemoveAndGet(0));
        }

        private IEnumerable<Link> LoadAllLinks()
        {
            var temp = new List<Link>();
            for (var i = 0; i <= _network.NumberOfLinks + 1; i++)
            {
                if (_fileLines[0].Equals(Separator))
                {
                    _fileLines.RemoveAt(0);
                    break;
                }
                temp.Add(GetOneLink(_fileLines.RemoveAndGet(0)));
            }

            return temp;
        }

        private static Link GetOneLink(string line)
        {
            var parameters = Array.ConvertAll(line.Split(null), int.Parse);

            return new Link()
            {
                StartNode = parameters[0],
                EndNode = parameters[1],
                NumberOfModules = parameters[2],
                ModuleCost = parameters[3],
                CapacityPerModule = parameters[4]
            };
        }

        private int GetNumberOfDemands()
        {
            if (string.IsNullOrWhiteSpace(_fileLines[0]))
                _fileLines.RemoveAt(0);
            return int.Parse(_fileLines.RemoveAndGet(0));
        }

        private IEnumerable<Demand> LoadAllDemands()
        {
            var temp = new List<Demand>();
            if (string.IsNullOrWhiteSpace(_fileLines[0]))
                _fileLines.RemoveAt(0);

            for (var i = 0; i <= _fileLines.Count; i++)
            {
                if (_fileLines[0].Equals(Separator))
                {
                    _fileLines.RemoveAt(0);
                    break;
                }
                temp.Add(GetOneDemand(GetDemandSection()));
            }

            return temp;
        }

        private List<string> GetDemandSection()
        {
            var temp = new List<string>();
            while (true)
            {
                if (string.IsNullOrWhiteSpace(_fileLines[0]))
                {
                    _fileLines.RemoveAt(0);
                    break;
                }

                temp.Add(_fileLines.RemoveAndGet(0));
            }

            return temp;
        }

        private Demand GetOneDemand(List<string> demandSection)
        {
            if (demandSection == null) return null;
            var firstLine = Array.ConvertAll(demandSection[0].Split(null), int.Parse);
            return new Demand
            {
                Id = ++_demandId,
                StartNode = firstLine[0],
                EndNode = firstLine[1],
                Volume = firstLine[2],
                NumberOfDemandPaths = int.Parse(demandSection[1].Trim()),
                DemandPaths = GetDemandPath(demandSection.Skip(2))
            };
        }

        private IEnumerable<DemandPath> GetDemandPath(IEnumerable<string> lines)
        {
            var id = 0;
            foreach (var path in lines)
            {
                var paths = Array.ConvertAll(path.Split(null), int.Parse);

                yield return new DemandPath()
                {
                    Id = ++id,
                    Paths = paths
                };
            }
        }
    }
}
