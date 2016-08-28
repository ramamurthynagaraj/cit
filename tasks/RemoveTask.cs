using System;
using cit.tasks.commands;
using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class RemoveTask : ITask
    {
        Action<string> _logger;
        public RemoveTask(Action<string> logger)
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
            if (command.EnvName == Constants.DefaultEnvName)
            {
                _logger("Removing the key from default environment will delete it from other environment as well.");
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
            if(!Validators.KeyNameValidator.IsMatch(command.KeyName))
            {
                _logger($"Keyname: '{command.KeyName}' should contain alphanumeric characters. E.g key, key01, key_1");
                return 1;
            }
            Store.Remove(command.EnvName, command.KeyName);
            _logger($"Key: {command.KeyName} deleted from Environment: {command.EnvName} successfully.");
            return 0;
        }

        private RemoveCommand Parse(string[] commands)
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