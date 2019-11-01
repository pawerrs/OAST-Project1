using System;

namespace OAST.Project1.Services.Network
{
    public class NetworkService : INetworkService
    {
        public NetworkService()
        {
            
        }

        public int CalculateLinkSize(int linkLoad, int linkModule)
        {
            return (linkLoad + linkModule - 1) / linkModule;
        }
    }
}
