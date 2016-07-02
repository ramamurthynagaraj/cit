using System;

using cit.utilities;
using cit.tasks;
using System.Collections.Generic;

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
            return GetTaskFor(args[0]).HandleCommand(args);
        }

        private static ITask GetTaskFor(string command)
        {
            var tasks = new Dictionary<string, ITask>
            {
                {"init", new InitTask(Console.WriteLine)},
                {"clean", new CleanTask(Console.WriteLine)},
                {"add", new AddTask(Console.WriteLine)},
                {"remove", new RemoveTask(Console.WriteLine)},
                {"copy", new CopyTask(Console.WriteLine)}
            };
            if(tasks.ContainsKey(command))
            {
                return tasks[command];
            }
            return new HelpTask(Console.WriteLine);
        }
    }
}
