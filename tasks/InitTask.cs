using System;
using cit.tasks.commands;
using cit.utilities;

namespace cit.tasks
{
    class InitTask
    {
        Action<string> _logger;
        public InitTask(Action<string> logger)
        {
            _logger = logger;
        }

        public int HandleCommand(InitCommand command)
        {
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
    }
}