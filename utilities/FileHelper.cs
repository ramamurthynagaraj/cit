namespace cit.utilities
{
    class FileHelper
    {
        public static string GetFileNameFor(string envName)
        {
            return $"{envName}.json";
        }
    }
}