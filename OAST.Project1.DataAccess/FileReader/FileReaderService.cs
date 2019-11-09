using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OAST.Project1.DataAccess.FileReader
{
    public class FileReaderService: IFileReaderService
    {
        public List<string> ReadFile(string pathToTheFile)
        {
            if (File.Exists(pathToTheFile))
                return File.ReadAllLines(pathToTheFile).ToList();

            Console.WriteLine("File does not exist");
            return null;
        }
    }
}
