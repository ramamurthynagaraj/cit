using System.IO;

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
    }
}