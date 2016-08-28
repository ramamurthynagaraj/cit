using System;
using cit.tasks.commands;
using cit.utilities;

namespace cit.tasks
{
    class InitTask : ITask
    {
        Action<string> _logger;
        public InitTask(Action<string> logger)
        {
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            var command = Parse(commands);
            if(command == null)
            {
                return 1;
            }
            if (command.EnvName == Constants.DefaultEnvName && !Store.IsEnvironmentExists(Constants.DefaultEnvName))
            {
                _logger($"Default environment not found.");
                Store.Create(Constants.DefaultEnvName);
                _logger($"Environment: default created succesfully.");
                return 0;
            }
            return new CopyTask(_logger).CopyEnvironment(Constants.DefaultEnvName, command.EnvName);
        }

        private InitCommand Parse(string[] commands)
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
    }
}