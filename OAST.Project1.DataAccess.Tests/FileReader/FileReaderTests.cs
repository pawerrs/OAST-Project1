
using System;
using System.IO;
using OAST.Project1.DataAccess.FileReader;
using Xunit;

namespace OAST.Project1.DataAccess.Tests.FileReader
{
    public class FileReaderTests
    {
        private readonly IFileReaderService _fileReaderService;

        public FileReaderTests()
        {
            _fileReaderService = new FileReaderService();
        }

        [Fact]
        public void File_Exist()
        {
            // Arrange
            var pathToBigTextFile = Path.Combine(Environment.CurrentDirectory, @"Input Data\", "net12_1.txt");
            var pathToSmallTextFile = Path.Combine(Environment.CurrentDirectory, @"Input Data\", "net4.txt");

            //Act
            var fileLinesOfBigTextFile = _fileReaderService.ReadFile(pathToBigTextFile);
            var fileLinesOfSmallTextFile = _fileReaderService.ReadFile(pathToSmallTextFile);

            //Assert
            Assert.NotNull(fileLinesOfBigTextFile);
            Assert.NotNull(fileLinesOfSmallTextFile);
        }

        [Fact]
        public void File_Does_Not_Exist()
        {
            // Arrange
            var pathToBigTextFile = Path.Combine(Environment.CurrentDirectory, @"Input Data\", "net5.txt");

            //Act
            var fileLinesOfBigTextFile = _fileReaderService.ReadFile(pathToBigTextFile);

            //Assert
            Assert.Null(fileLinesOfBigTextFile);
        }
    }
}
