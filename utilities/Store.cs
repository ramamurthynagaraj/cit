using System;
using System.IO;
using System.Linq;
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

        public static void Clean(string envName)
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

        public static void Remove(string envName, string keyName)
        {
            var fileName = GetFileNameFor(envName);
            var fileContent = File.ReadAllText(fileName);
            var existingJson = JObject.Parse(string.IsNullOrEmpty(fileContent) ? @"{'cit': {}}": fileContent);
            var allKeys = existingJson.GetValue("cit") as JObject;
            if(allKeys.Properties().Any(p => p.Name == keyName))
            {
                allKeys.Property(keyName).Remove();
            }
            File.WriteAllText(fileName, existingJson.ToString());
        }
    }
}

