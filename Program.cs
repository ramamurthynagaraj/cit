using System;

using cit.utilities;
using cit.tasks;

namespace cit
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine($"Welcome to CIt. Version: {Constants.Version} .");                
            }
            if (args[0] == "init")
            {
                return new InitTask(Console.WriteLine).HandleCommand(args);
            }
            if (args[0] ==  "clean")
            {
                return new CleanTask(Console.WriteLine).HandleCommand(args);
            }
            if (args[0] ==  "add")
            {
                return new AddTask(Console.WriteLine).HandleCommand(args);
            }
            if (args[0] ==  "remove")
            {
                return new RemoveTask(Console.WriteLine).HandleCommand(args);
            }
            Console.WriteLine("Provided command not found.");
            return 0;
        }
    }
}
