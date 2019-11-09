using System;
using OAST.Project1.Services.Helpers;
using Xunit;

namespace OAST.Project1.Services.Tests.Helpers
{
    public class NetworkServiceTests
    {
        [Fact]
        public void CalculateLinkSize_ProperLinkValues_ShouldReturnComputedLinkSize()
        {
            // Arrange
            var linkLoad = 10;

            var linkModule1 = 1;
            var linkModule2 = 2;
            var linkModule3 = 3;
            var linkModule4 = 4;

            // Act
            var linkSize1 = NetworkHelper.CalculateLinkSize(linkLoad, linkModule1);
            var linkSize2 = NetworkHelper.CalculateLinkSize(linkLoad, linkModule2);
            var linkSize3 = NetworkHelper.CalculateLinkSize(linkLoad, linkModule3);
            var linkSize4 = NetworkHelper.CalculateLinkSize(linkLoad, linkModule4);

            // Assert
            Assert.Equal(10, linkSize1);
            Assert.Equal(5, linkSize2);
            Assert.Equal(4, linkSize3);
            Assert.Equal(3, linkSize4);
        }

        [Fact]
        public void CalculateLinkSize_LinkModuleEqual0_ShouldThrowArgumentException()
        {
            // Arrange
            var linkLoad = 10;
            var linkModule = 0;
            
            // Act & Assert
            Assert.Throws<ArgumentException>(() => NetworkHelper.CalculateLinkSize(linkLoad, linkModule));
        }
    }
}
