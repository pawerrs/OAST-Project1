
using System;
using System.IO;
using OAST.Project1.Models.Common;
using System.Text.Json;
using OAST.Project1.Common.Extensions;

namespace OAST.Project1.DataAccess.OutputWriter
{
    public class OutputWriter
    {
        public void SaveOutputToTheFile<T>(T result, MenuOptions menuOptions)
        {
            File.WriteAllText(Path.Combine(GetFilePath(), @"Output\", CreateFileName(menuOptions)), Serialize(result));
        }

        private static string Serialize<T>(T value)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(value, options);
        }

        private static string GetFilePath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        }

        private static string CreateFileName(MenuOptions menuOptions)
        {
            return ($"{menuOptions.AlgorithmType}_{menuOptions.ProblemType}_ {Extensions.GetFileName(menuOptions.FileName)}.json");
        }
    }
}
