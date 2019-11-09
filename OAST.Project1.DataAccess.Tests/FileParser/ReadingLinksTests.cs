using System.Collections.Generic;
using FluentAssertions;
using OAST.Project1.DataAccess.FileParser;
using OAST.Project1.DataAccess.FileReader;
using OAST.Project1.Models.Topology;
using Xunit;

namespace OAST.Project1.DataAccess.Tests.FileParser
{
    public class ReadingLinksTests
    {
        private readonly IFileParserService _fileParserService;
        private const string TestedFile = "net12_1.txt";
        private readonly List<string> _fileLines;

        public ReadingLinksTests()
        {
            IFileReaderService fileReaderService = new FileReaderService();
            _fileParserService = new FileParserService(fileReaderService, TestedFile);
            _fileLines = _fileParserService.GetConfigurationLines();
        }

        [Fact]
        public void GetNumberOfLinks_ShouldReturnNumberOfLinks()
        {
            //Arrange
            var expectedNumberOfLinks = 18;

            //Act
            var numberOfLinks = _fileParserService.GetNumberOfLinks();

            //Assert
            Assert.Equal(expectedNumberOfLinks, numberOfLinks);
        }

        [Fact]
        public void GetOneLink_ShouldReturnProperLinkObject()
        {
            //Arrange
            var firstExpectedLinkObject = new Link(1,11,2,1,96);
            const int firstLinkLine = 1;

            var lastExpectedLinkObject = new Link(1, 6, 2, 1, 96); ;
            const int lastLinkLine = 18;

            //Act
            var firstLink = _fileParserService.GetOneLink(_fileLines[firstLinkLine]);
            var lastLink = _fileParserService.GetOneLink(_fileLines[lastLinkLine]);

            //Assert
            firstExpectedLinkObject.Should().BeEquivalentTo(firstLink);
            lastExpectedLinkObject.Should().BeEquivalentTo(lastLink);
        }

        [Fact]
        public void LoadAllLinks_ShouldReturnAllLinks()
        {
            //Arrange
            var links = new List<Link>
            {
                new Link(1, 11, 2, 1, 96),
                new Link(1, 3, 2, 1, 96),
                new Link(2, 3, 2, 1, 96),
                new Link(2, 8, 2, 1, 96),
                new Link(2, 11, 2, 1, 96),
                new Link(3, 10, 2, 1, 96),
                new Link(4, 5, 2, 1, 96),
                new Link(4, 7, 2, 1, 96),
                new Link(4, 12, 2, 1, 96),
                new Link(5, 9, 2, 1, 96),
                new Link(5, 11, 2, 1, 96),
                new Link(6, 9, 2, 1, 96),
                new Link(6, 11, 2, 1, 96),
                new Link(7, 11, 2, 1, 96),
                new Link(7, 12, 2, 1, 96),
                new Link(8, 10, 2, 1, 96),
                new Link(8, 12, 2, 1, 96),
                new Link(1, 6, 2, 1, 96)
            };

            //Act
            var allLinks = _fileParserService.LoadAllLinks();

            //Assert
            links.Should().AllBeAssignableTo<Link>();
            links.Should().AllBeOfType<Link>();
            links.Should().BeEquivalentTo(allLinks);
        }
    }
}
