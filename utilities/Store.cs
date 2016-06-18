using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace cit.utilities
{
    class Store
    {
        private static string GetFileNameFor(string envName)
        {
            return $"{envName}.json";
        }

        public static bool IsEnvironmentExists(string envName)
        {
            return File.Exists(GetFileNameFor(envName));
        }

        public static void Create(string envName)
        {
            File.Create(GetFileNameFor(envName));
        }

        public static void Delete(string envName)
        {
            File.Delete(GetFileNameFor(envName));
        }

        public static void Add(string envName, string keyName, string value)
        {
            var fileName = GetFileNameFor(envName);
            var fileContent = File.ReadAllText(fileName);
            var existingJson = JObject.Parse(string.IsNullOrEmpty(fileContent) ? @"{'cit': {}}": fileContent);
            var newItem = JObject.Parse($"{{'cit': {{'{keyName}': '{value}'}}}}");
            existingJson.Merge(newItem, new JsonMergeSettings{
                MergeArrayHandling = MergeArrayHandling.Union
            });
            File.WriteAllText(fileName, existingJson.ToString());
        }
    }
}

