using System;
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
            if (commands.Length != 3)
            {
                _logger("Environment name and key name not specified correctly. Please specify properly. E.g 'cit remove staging key'");
                return 1;
            }
            var envName = commands[1];
            var keyName =  commands[2];
            if (envName == Constants.DefaultEnvName)
            {
                _logger("Removing the key from default environment will delete it from other environment as well.");
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
            if(!Validators.KeyNameValidator.IsMatch(keyName))
            {
                _logger($"Keyname: '{keyName}' should contain alphanumeric characters. E.g key, key01, key_1");
                return 1;
            }
            Store.Remove(envName, keyName);
            _logger($"Key: {keyName} deleted from Environment: {envName} successfully.");
            return 0;
        }
    }
}