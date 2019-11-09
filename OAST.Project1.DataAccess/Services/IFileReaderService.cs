using System.Collections.Generic;

namespace OAST.Project1.DataAccess.Services
{
    public interface IFileReaderService
    {
        List<string> ReadFile(string pathToTheFile);
    }
}
