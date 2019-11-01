using OAST.Project1.Services.Network;
using Xunit;

namespace OAST.Project1.Services.Tests.Network
{
    public class NetworkServiceTests
    {
        private readonly INetworkService _networkService;

        public NetworkServiceTests()
        {
            _networkService = new NetworkService();
        }

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
            var linkSize1 = _networkService.CalculateLinkSize(linkLoad, linkModule1);
            var linkSize2 = _networkService.CalculateLinkSize(linkLoad, linkModule2);
            var linkSize3 = _networkService.CalculateLinkSize(linkLoad, linkModule3);
            var linkSize4 = _networkService.CalculateLinkSize(linkLoad, linkModule4);

            // Assert
            Assert.Equal(10, linkSize1);
            Assert.Equal(5, linkSize2);
            Assert.Equal(4, linkSize3);
            Assert.Equal(3, linkSize4);
        }
    }
}
