using System;
using System.Collections.Generic;
using System.Linq;
using cit.tasks.commands;
using cit.utilities;

namespace cit.cli
{
    public class Parser
    {
        Action<string> _logger;

        public Parser(Action<string> logger){
            _logger = logger;
        }

        public AddCommand TryAddCommand(string[] commands)
        {
            if(commands.Length != 4){
                _logger("Parameters missig, mention all parameters. E.g 'cit add staging key value', 'cit add staging key \"value with spaces\"'");
                return null;
            }

            return new AddCommand {
                EnvName = commands[1],
                KeyName = commands[2],
                ItemValue = commands[3]
            };
        }

        public ApplyCommand TryApplyCommand(string[] commands)
        {
            if(commands.Length == 1){
                _logger("Parameters missing, mention the environment(s) that needs to be applied to file(s). E.g 'cit apply staging -f file1', 'cit apply testing staging -f file1'");
                return null;
            }
            var applyArguments = commands.Skip(1).ToList();
            var environments = applyArguments.TakeWhile(arg => !"-f".Equals(arg)).ToList(); 
            
            var files = new List<string>();           
            var fileFlagIndex = applyArguments.FindIndex(arg => "-f".Equals(arg));
            if(fileFlagIndex  != -1)
            {
                files = applyArguments.Skip(fileFlagIndex + 1).ToList();      
            }

            return new ApplyCommand {
                Files = files,
                Environments = environments
            };
        }

        public CleanCommand TryCleanCommand(string[] commands)
        {
            if (commands.Length != 2)
            {
                _logger("Environment name not specified. Please specify one. E.g 'cit clean staging'");
                return null;
            }
            return new CleanCommand {
                EnvName = commands[1]
            };
        }

        public CopyCommand TryCopyCommand(string[] commands)
        {
            if (commands.Length != 3)
            {
                _logger("Wrong number of parameters specified");
                return null;
            }
            return new CopyCommand {
                FromEnvName = commands[0],
                ToEnvName = commands[1]
            };
        }

        public InitCommand TryInitCommand(string[] commands)
        {
            if(commands.Length != 1 && commands.Length != 2)
            {
                _logger("Wrong number of parameters specified");
                return null;
            }
            var envName = Constants.DefaultEnvName;
            if(commands.Length == 2)
            {
                envName = commands[1];
            }
            return new InitCommand{
                EnvName = envName
            };
        }

        public RemoveCommand TryRemoveCommand(string[] commands)
        {
            if (commands.Length != 3)
            {
                _logger("Environment name and key name not specified correctly. Please specify properly. E.g 'cit remove staging key'");
                return null;
            }
            return new RemoveCommand {
                EnvName = commands[1],
                KeyName = commands[2]
            };
        }
    }
}