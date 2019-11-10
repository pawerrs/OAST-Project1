using System.Collections.Generic;
using OAST.Project1.Common.Enums;

namespace OAST.Project1.DataAccess.FileReader
{
    public interface IFileReaderService
    {
        List<string> ReadFile(string pathToTheFile);
        string GetFileName(FileName fileName);
    }
}
