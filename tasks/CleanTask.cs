using System;
using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class CleanTask : ITask
    {
        Action<string> _logger;
        public CleanTask(Action<string> logger)
        {
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            if (commands.Length != 2)
            {
                _logger("Environment name not specified. Please specify one. E.g 'cit clean staging'");
                return 1;
            }
            var envName = commands[1];
            if (envName == Constants.DefaultEnvName)
            {
                _logger("Cannot delete default environment name. Delete the directory instead.");
                return 1;
            }
            if (!Validators.EnvNameValidator.IsMatch(envName))
            {
                _logger("Environment name cannot be empty and should contain alphanumeric characters. E.g staging, staging01, staging_1");
                return 1;
            }
            if (!Store.IsEnvironmentExists(envName))
            {
                _logger($"Environment: {envName} does not exists. Please verify the supplied environment name.");
                return 1;
            }
            Store.Delete(envName);
            _logger($"Environment: {envName} deleted successfully.");
            return 0;
        }
    }
}