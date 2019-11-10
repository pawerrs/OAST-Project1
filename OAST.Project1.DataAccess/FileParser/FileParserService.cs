﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Topology;

namespace OAST.Project1.DataAccess.FileParser
{
    public class FileParserService: IFileParserService
    {
        private List<string> _fileLines;
        private readonly Network _network;
        private const string Separator = "-1";
        private int _currentLineNumber;
        private int _demandId;

        public FileParserService(IFileReaderService fileReaderService, string fileName)
        {
            _fileLines =
                fileReaderService.ReadFile(Path.Combine(Environment.CurrentDirectory, @"Input Data\", fileName));
            _network = new Network();
        }

        public Network LoadTopology(List<string> fileLines)
        {
            _fileLines = fileLines;
            _network.NumberOfLinks = GetNumberOfLinks();
            _network.Links = LoadAllLinks();
            _network.NumberOfDemands = GetNumberOfDemands();
            _network.Demands = LoadAllDemands();
            return _network;
        }

        public int GetNumberOfLinks()
        {
            return int.Parse(_fileLines[0]);
        }

        public IEnumerable<Link> LoadAllLinks()
        {
            _currentLineNumber = 1;
            var temp = new List<Link>();

            for (var i = 0; i <= _fileLines.Count; i++)
            {
                if (_fileLines[_currentLineNumber].Equals(Separator))
                {
                    _currentLineNumber++;
                    break;
                }
                temp.Add(GetOneLink(_fileLines[_currentLineNumber]));
                _currentLineNumber++;
            }
            return temp;
        }

        public Link GetOneLink(string line)
        {
            var parameters = Array.ConvertAll(line.Split(null), int.Parse);

            return new Link(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
        }

        public int GetNumberOfDemands()
        {
            if (string.IsNullOrWhiteSpace(_fileLines[_currentLineNumber]))
                _currentLineNumber++;
            return int.Parse(_fileLines[_currentLineNumber++]);
        }

        public IEnumerable<Demand> LoadAllDemands()
        {
            var temp = new List<Demand>();
            if (string.IsNullOrWhiteSpace(_fileLines[_currentLineNumber]))
                _currentLineNumber++;

            for (var i = 0; i <= _fileLines.Count; i++)
            {
                if (temp.Count == _network.NumberOfDemands)
                {
                    break;
                }
                temp.Add(GetOneDemand(GetDemandSection()));
            }

            return temp;
        }

        public List<string> GetDemandSection()
        {
            var section = new List<string>();
            while (true)
            {
                if (string.IsNullOrWhiteSpace(_fileLines[_currentLineNumber]))
                {
                    _currentLineNumber++;
                    break;
                }

                section.Add(_fileLines[_currentLineNumber++].Trim());
            }
            return section;
        }

        public Demand GetOneDemand(List<string> demandSection)
        {
            if (demandSection == null) return null;
            var firstLine = Array.ConvertAll(demandSection[0].Split(null), int.Parse);

            return new Demand(++_demandId, firstLine[0], firstLine[1], firstLine[2],
                int.Parse(demandSection[1].Trim()), GetDemandPath(demandSection.Skip(2)));
        }

        public IEnumerable<DemandPath> GetDemandPath(IEnumerable<string> lines)
        {
            var id = 0;
            foreach (var path in lines)
            {
                var paths = Array.ConvertAll(path.Split(null), int.Parse);

                yield return new DemandPath(++id, paths);
            }
        }

        public List<string> GetConfigurationLines()
        {
            return _fileLines;
        }

        public void SetCurrentLineNumber(int number)
        {
            _currentLineNumber = number;
        }

        public void SetNumberOfDemands(int number)
        {
            _network.NumberOfDemands = number;
        }
    }
}