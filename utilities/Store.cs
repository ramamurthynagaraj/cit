using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static cit.utilities.Constants;

namespace cit.utilities
{
    class Store
    {
        public static string AppName = "cit";
        public static string SecureKeys = $"{AppName}_SecureKeys";
        private const string MetaDataFile = "cit-environments.json";
        private static string GetFileNameFor(string envName)
        {
            return $"{envName}.json";
        }

        public static bool IsEnvironmentExists(string envName)
        {
            return File.Exists(GetFileNameFor(envName));
        }

        public static void Copy(string fromEnv, string toEnv)
        {
            File.Copy(GetFileNameFor(fromEnv), GetFileNameFor(toEnv));
            AddNewEnvironmentEntry(toEnv);
        }

        public static void Create(string envName)
        {
            File.Create(GetFileNameFor(envName)).Dispose();
            AddNewEnvironmentEntry(envName);
        }

        private static void AddNewEnvironmentEntry(string envName)
        {
            var defaultEnvJson = GetJsonForFile(MetaDataFile);
            var newEnvironment = JObject.Parse($"{{'environments': ['{envName}']}}");
            defaultEnvJson.Merge(newEnvironment, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
            File.WriteAllText(MetaDataFile, defaultEnvJson.ToString());
        }

        private static string GetAppKeyFor(bool isSecure)
        {
            return isSecure ? SecureKeys : AppName;
        }
        public static Dictionary<string, string> GetFinalValuesFor(List<string> environments, bool isSecure)
        {
            var appKey = GetAppKeyFor(isSecure);
            if(!environments.Any(env => env == DefaultEnvName))
            {
                environments.Insert(0, DefaultEnvName);
            }
            var finalJson = environments.Distinct().Select(env => GetJsonForFile(GetFileNameFor(env)).GetValue(appKey) as JObject)
                .Where(json => json != null)
                .Aggregate(JObject.Parse(@"{}"), (firstJson, secondJson) => {
                    firstJson.Merge(secondJson, new JsonMergeSettings{
                        MergeArrayHandling = MergeArrayHandling.Replace
                    });
                    return firstJson;
                });
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(finalJson.ToString());
        }

        public static void Delete(string envName)
        {
            File.Delete(GetFileNameFor(envName));
            RemoveEnvironmentEntry(envName);
        }

        private static void RemoveEnvironmentEntry(string envName)
        {
            var defaultEnvJson = GetJsonForFile(MetaDataFile);
            defaultEnvJson["environments"].Where(env => env.Value<string>() == envName)
                .ToList().ForEach(env => env.Remove());
            File.WriteAllText(MetaDataFile, defaultEnvJson.ToString());
        }

        public static void Add(string envName, string keyName, string value, bool isSecure)
        {
            if(envName != DefaultEnvName && !HasDefaultValueFor(keyName))
            {
                throw new NoDefaultValueFoundException("Cannot override without a default value. Please add default value first.");
            }
            Remove(envName, keyName);
            var fileName = GetFileNameFor(envName);
            var existingJson = GetJsonForFile(fileName);
            var appKeyName = GetAppKeyFor(isSecure);
            var newItem = JObject.Parse($"{{'{appKeyName}': {{'{keyName}': '{value}'}}}}");
            existingJson.Merge(newItem, new JsonMergeSettings{
                MergeArrayHandling = MergeArrayHandling.Union
            });
            File.WriteAllText(fileName, existingJson.ToString());
        }

        private static bool HasDefaultValueFor(string keyName)
        {
            var defaultJson = GetJsonForFile(GetFileNameFor(DefaultEnvName));
            return defaultJson[AppName]?[keyName] != null || defaultJson[SecureKeys]?[keyName] != null;
        }

        public static void Remove(string envName, string keyName)
        {
            Remove(envName, keyName, false);
            Remove(envName, keyName, true);
        }

        private static void Remove(string envName, string keyName, bool isSecure)
        {
            var fileName = GetFileNameFor(envName);
            var existingJson = GetJsonForFile(fileName);
            var appKeyName = GetAppKeyFor(isSecure);            
            var allKeys = existingJson.GetValue(appKeyName) as JObject;
            if(allKeys == null)
            {
                return;
            }
            if(allKeys.Properties().Any(p => p.Name == keyName))
            {
                allKeys.Property(keyName).Remove();
            }
            File.WriteAllText(fileName, existingJson.ToString());
        }

        private static JObject GetJsonForFile(string fileName)
        {
            var fileContent = string.Empty;
            if(File.Exists(fileName))
            {
                fileContent = File.ReadAllText(fileName);
            }
            return JObject.Parse(string.IsNullOrEmpty(fileContent) ? @"{}": fileContent);
        }
    }

    public class NoDefaultValueFoundException : Exception
    {
        public NoDefaultValueFoundException(string message) : base(message)
        {
        }
    }
}

