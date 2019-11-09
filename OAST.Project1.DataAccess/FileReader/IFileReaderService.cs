using System.Collections.Generic;

namespace OAST.Project1.DataAccess.FileReader
{
    public interface IFileReaderService
    {
        List<string> ReadFile(string pathToTheFile);
    }
}
