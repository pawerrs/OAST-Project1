
using System;
using System.IO;
using OAST.Project1.DataAccess.Services;
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

        [Fact]
        public void Big_File_Has_Correct_Number_Of_Lines()
        {
            // Arrange
            var pathToBigTextFile = Path.Combine(Environment.CurrentDirectory, @"Input Data\", "net12_1.txt");

            //Act
            var fileLinesOfBigTextFile = _fileReaderService.ReadFile(pathToBigTextFile);

            //Assert
            Assert.Equal(460, fileLinesOfBigTextFile.Count);
        }

        [Fact]
        public void Small_File_Has_Correct_Number_Of_Lines()
        {
            // Arrange
            var pathToSmallTextFile = Path.Combine(Environment.CurrentDirectory, @"Input Data\", "net4.txt");

            //Act
            var fileLinesOfSmallTextFile = _fileReaderService.ReadFile(pathToSmallTextFile);

            //Assert
            Assert.Equal(44, fileLinesOfSmallTextFile.Count);
        }
    }
}
