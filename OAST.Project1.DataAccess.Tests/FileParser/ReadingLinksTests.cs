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
            int firstId = 1;
            var firstExpectedLinkObject = new Link(firstId, 1,11,2,1,96);
            const int firstLinkLine = 1;

            int lastId = 18;
            var lastExpectedLinkObject = new Link(lastId, 1, 6, 2, 1, 96); ;
            const int lastLinkLine = 18;

            //Act
            var firstLink = _fileParserService.GetOneLink(_fileLines[firstLinkLine], ref firstId);
            var lastLink = _fileParserService.GetOneLink(_fileLines[lastLinkLine], ref lastId);

            //Assert
            firstExpectedLinkObject.Should().BeEquivalentTo(firstLink);
            lastExpectedLinkObject.Should().BeEquivalentTo(lastLink);
        }

        [Fact]
        public void LoadAllLinks_ShouldReturnAllLinks()
        {
            //Arrange
            _fileParserService.SetCurrentLineNumber(1);
            var links = new List<Link>
            {
                new Link(1, 1, 11, 2, 1, 96),
                new Link(2, 1, 3, 2, 1, 96),
                new Link(3, 2, 3, 2, 1, 96),
                new Link(4, 2, 8, 2, 1, 96),
                new Link(5, 2, 11, 2, 1, 96),
                new Link(6, 3, 10, 2, 1, 96),
                new Link(7,4, 5, 2, 1, 96),
                new Link(8, 4, 7, 2, 1, 96),
                new Link(9, 4, 12, 2, 1, 96),
                new Link(10, 5, 9, 2, 1, 96),
                new Link(11, 5, 11, 2, 1, 96),
                new Link(12, 6, 9, 2, 1, 96),
                new Link(13, 6, 11, 2, 1, 96),
                new Link(14, 7, 11, 2, 1, 96),
                new Link(15, 7, 12, 2, 1, 96),
                new Link(16, 8, 10, 2, 1, 96),
                new Link(17, 8, 12, 2, 1, 96),
                new Link(18, 1, 6, 2, 1, 96)
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
