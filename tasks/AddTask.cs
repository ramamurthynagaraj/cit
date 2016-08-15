using System;

using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class AddTask : ITask
    {
        Action<string> _logger;

        public AddTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            if(commands.Length != 4){
                _logger("Parameters missig, mention all parameters. E.g 'cit add staging key value', 'cit add staging key \"value with spaces\"'");
                return 1;
            }
            var envName = commands[1];
            var keyName =  commands[2];
            var itemValue = commands[3];
            if(!Store.IsEnvironmentExists(envName))
            {
                _logger($"Environment: {envName} does not exist. Create one using 'cit init {envName}' before proceeding.");
                return 1;
            }
            if(!Validators.KeyNameValidator.IsMatch(keyName))
            {
                _logger($"Keyname: '{keyName}' should contain alphanumeric characters. E.g key, key01, key_1");
                return 1;
            }
            try
            {
                Store.Add(envName, keyName, itemValue);
            }
            catch(NoDefaultValueFoundException ex)
            {
                _logger(ex.Message);
                return 1;
            }
            _logger($"Key: {keyName}, Value: {itemValue} added to Environment: {envName} successfully.");
            return 0;
        }
    }
}