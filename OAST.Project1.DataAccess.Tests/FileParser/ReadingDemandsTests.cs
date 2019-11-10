
using System.Collections.Generic;
using FluentAssertions;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Topology;
using Xunit;

namespace OAST.Project1.DataAccess.Tests.FileParser
{
    public class ReadingDemandsTests
    {
        private readonly IFileParserService _fileParserService;
        private const string TestedFile = "net12_1.txt";
        private List<string> _fileLines;

        public ReadingDemandsTests()
        {
            IFileReaderService fileReaderService = new FileReaderService();
            _fileParserService = new FileParserService(fileReaderService, TestedFile);
            _fileLines = _fileParserService.GetConfigurationLines();
        }

        [Fact]
        public void GetNumberOfDemands_ShouldReturnNUmberOfAllDemands()
        {
            //Arrange
            int expectedNumberOfDemands = 66;
            _fileParserService.SetCurrentLineNumber(21);

            //Act
            var numberOfDemands = _fileParserService.GetNumberOfDemands();

            //Assert
            Assert.Equal(expectedNumberOfDemands, numberOfDemands);
        }

        [Fact]
        public void GetDemandSection_ShouldReturnListOfStrings()
        {
            //Arrange
            _fileParserService.SetCurrentLineNumber(23);
            const int expectedLines = 6;
            var expectedElements = new List<string>() {"1 2 18", "4", "1 1 5", "2 2 3", "3 18 13 5", "4 2 6 16 4"};

            //Act
            var demandSection = _fileParserService.GetDemandSection();

            //Assert
            Assert.NotNull(demandSection);
            demandSection.Should().NotBeEmpty()
                .And.HaveCount(expectedLines)
                .And.Equal(expectedElements)
                .And.ContainItemsAssignableTo<string>();
        }

        [Fact]
        public void GetOneDemand_ShouldReturnDemand()
        {
            //Arrange 
            var expectedDemand = new Demand(1, 1,2,18,4, new List<DemandPath>
            {
                new DemandPath(1, new []{1, 1, 5}),
                new DemandPath(2, new []{2, 2, 3}),
                new DemandPath(3, new []{3, 18, 13, 5}),
                new DemandPath(4, new []{4, 2, 6, 16, 4}),
            } );
            var lines = new List<string>() { "1 2 18", "4", "1 1 5", "2 2 3", "3 18 13 5", "4 2 6 16 4" };

            //Act
            var demand = _fileParserService.GetOneDemand(lines);

            //Assert
            Assert.NotNull(demand);
            demand.Should().BeAssignableTo<Demand>()
                .And.BeOfType<Demand>()
                .And.BeEquivalentTo(expectedDemand);
        }

        [Fact]
        public void GetAllDemands_ShouldReturnAllDemands()
        {
            //Arrange
            const int expectedNumberOfDemands = 66;
            _fileParserService.SetCurrentLineNumber(23);
            _fileParserService.SetNumberOfDemands(expectedNumberOfDemands);

            //Act
            var allDemands = _fileParserService.LoadAllDemands();

            //Assert
            Assert.NotNull(allDemands);
            allDemands.Should().AllBeAssignableTo<Demand>()
                .And.HaveCount(expectedNumberOfDemands)
                .And.NotContain(" ")
                .And.AllBeOfType<Demand>();
        }
    }
}
