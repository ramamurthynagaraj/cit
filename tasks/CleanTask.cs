using System;
using cit.tasks.commands;
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
            var command = Parse(commands);
            if (command == null) 
            {
                return 1;
            }
            if (command.EnvName == Constants.DefaultEnvName)
            {
                _logger("Cannot delete default environment name. Delete the directory instead.");
                return 1;
            }
            if (!Validators.EnvNameValidator.IsMatch(command.EnvName))
            {
                _logger("Environment name cannot be empty and should contain alphanumeric characters. E.g staging, staging01, staging_1");
                return 1;
            }
            if (!Store.IsEnvironmentExists(command.EnvName))
            {
                _logger($"Environment: {command.EnvName} does not exists. Please verify the supplied environment name.");
                return 1;
            }
            Store.Delete(command.EnvName);
            _logger($"Environment: {command.EnvName} deleted successfully.");
            return 0;
        }

        private CleanCommand Parse(string[] commands)
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
    }
}