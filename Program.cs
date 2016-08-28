using System;

using cit.utilities;
using cit.tasks;
using System.Collections.Generic;
using cit.cli;

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
            return GetTaskFor(args[0], new Parser(Console.WriteLine)).Invoke(args);
        }

        private static Func<string[], int> GetTaskFor(string command, Parser parser)
        {
            var tasks = new Dictionary<string, Func<string[], int>>
            {
                {"init", (args) => new InitTask(Console.WriteLine).HandleCommand(parser.TryInitCommand(args))},
                {"clean", (args) => new CleanTask(Console.WriteLine).HandleCommand(parser.TryCleanCommand(args))},
                {"add", (args) => new AddTask(Console.WriteLine).HandleCommand(parser.TryAddCommand(args))},
                {"apply", (args) => new ApplyTask(Console.WriteLine).HandleCommand(parser.TryApplyCommand(args))},
                {"remove", (args) => new RemoveTask(Console.WriteLine).HandleCommand(parser.TryRemoveCommand(args))},
                {"copy", (args) => new CopyTask(Console.WriteLine).HandleCommand(parser.TryCopyCommand(args))}
            };
            if(tasks.ContainsKey(command))
            {
                return tasks[command];
            }
            return (args) => new HelpTask(Console.WriteLine).HandleCommand(args);
        }
    }
}
