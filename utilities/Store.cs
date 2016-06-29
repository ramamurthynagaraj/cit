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
            File.Create(GetFileNameFor(envName)).Dispose();
            var defaultEnv = GetFileNameFor(Constants.DefaultEnvName);
            var defaultEnvJson = GetJsonForFile(defaultEnv);
            var newEnvironment = JObject.Parse($"{{'environment': ['{envName}']}}");
            defaultEnvJson.Merge(newEnvironment, new JsonMergeSettings{
                MergeArrayHandling = MergeArrayHandling.Union
            });
            File.WriteAllText(defaultEnv, defaultEnvJson.ToString());
        }

        public static void Clean(string envName)
        {
            File.Delete(GetFileNameFor(envName));
        }

        public static void Add(string envName, string keyName, string value)
        {
            var fileName = GetFileNameFor(envName);
            var existingJson = GetJsonForFile(fileName);
            var newItem = JObject.Parse($"{{'cit': {{'{keyName}': '{value}'}}}}");
            existingJson.Merge(newItem, new JsonMergeSettings{
                MergeArrayHandling = MergeArrayHandling.Union
            });
            File.WriteAllText(fileName, existingJson.ToString());
        }

        public static void Remove(string envName, string keyName)
        {
            var fileName = GetFileNameFor(envName);
            var existingJson = GetJsonForFile(fileName);
            var allKeys = existingJson.GetValue("cit") as JObject;
            if(allKeys.Properties().Any(p => p.Name == keyName))
            {
                allKeys.Property(keyName).Remove();
            }
            File.WriteAllText(fileName, existingJson.ToString());
        }

        private static JObject GetJsonForFile(string fileName)
        {
            var fileContent = File.ReadAllText(fileName);
            return JObject.Parse(string.IsNullOrEmpty(fileContent) ? @"{'cit': {}}": fileContent);
        }
    }
}

