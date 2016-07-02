using System;

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
            if(commands.Length != 1 && commands.Length != 2)
            {
                _logger("Wrong number of parameters specified");
                return 1;
            }
            if (!Store.IsEnvironmentExists(Constants.DefaultEnvName))
            {
                _logger($"Default environment not found.");
                Store.Create(Constants.DefaultEnvName);
                _logger($"Environment: default created succesfully.");
            }
            if(commands.Length == 1)
            {
                return 0;
            }
            var envName = commands[1];
            return new CopyTask(_logger).CopyEnvironment(Constants.DefaultEnvName, envName);
        }
    }
}