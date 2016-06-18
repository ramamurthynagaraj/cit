using System;

using cit.utilities;
using cit.utilities.validators;

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
            var envName = Constants.DefaultEnvName;
            if (commands.Length == 2)
            {
                envName = commands[1];
            }
            if (!Validators.EnvNameValidator.IsMatch(envName))
            {
                _logger($"Environment: '{envName}' name should contain alphanumeric characters. E.g staging, staging01, staging_1");
                return 1;
            }
            if (Store.IsEnvironmentExists(envName))
            {
                _logger($"Environment: {envName} already exists. Not creating it again.");
                return 1;
            }
            if (!Store.IsEnvironmentExists(Constants.DefaultEnvName) && envName != Constants.DefaultEnvName)
            {
                _logger($"Default environment not found.");
                Store.Create(Constants.DefaultEnvName);
                _logger($"Environment: default created succesfully.");
            }
            Store.Create(envName);
            _logger($"Environment: {envName} created succesfully.");
            return 0;
        }
    }
}