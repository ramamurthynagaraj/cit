using System;
using System.IO;

using cit.utilities;
using cit.utilities.validators;

namespace cit
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"Welcome to CIt. Version: {Constants.Version}");                
            }
            if (args.Length == 2)
            {
                if (args[0] == "init")
                {
                    if (Validators.FileNameValidators.IsMatch(args[1]))
                    {
                        var fileName = $"{args[1]}.json";
                        if (File.Exists(fileName))
                        {
                            Console.WriteLine("Environment already exists. Not creating it again.");
                        }
                        File.Create(fileName);                    
                    }
                    else
                    {
                        Console.WriteLine("Environment name should contain alphanumeric characters only. E.g staging, staging01, staging_1");
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
