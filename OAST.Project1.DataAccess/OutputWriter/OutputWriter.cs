
using System;
using System.IO;
using System.Text;
using OAST.Project1.Models.Common;
using System.Text.Json;

namespace OAST.Project1.DataAccess.OutputWriter
{
    public class OutputWriter
    {
        public void SaveOutputToTheFile<T>(T result, MenuOptions menuOptions)
        {
            File.WriteAllText(Path.Combine(GetFilePath(), @"Output\", CreateFileName(menuOptions).ToString()), Serialize(result));
        }

        public string Serialize<T>(T value)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(value, options);
        }

        public string GetFilePath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        }

        public StringBuilder CreateFileName(MenuOptions menuOptions)
        {
            return new StringBuilder(menuOptions.AlgorithmType + "_" + menuOptions.ProblemType + "_" + menuOptions.FileName +  ".json");
        }
    }
}
