using System;
using cit.tasks.commands;
using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class AddTask
    {
        Action<string> _logger;

        public AddTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(AddCommand command)
        {
            if(command == null)
            {
                return 1;
            }
            if(!Store.IsEnvironmentExists(command.EnvName))
            {
                _logger($"Environment: {command.EnvName} does not exist. Create one using 'cit init {command.EnvName}' before proceeding.");
                return 1;
            }
            if(!Validators.KeyNameValidator.IsMatch(command.KeyName))
            {
                _logger($"Keyname: '{command.KeyName}' should contain alphanumeric characters. E.g key, key01, key_1");
                return 1;
            }
            try
            {
                Store.Add(command.EnvName, command.KeyName, command.ItemValue);
            }
            catch(NoDefaultValueFoundException ex)
            {
                _logger(ex.Message);
                return 1;
            }
            _logger($"Key: {command.KeyName}, Value: {command.ItemValue} added to Environment: {command.EnvName} successfully.");
            return 0;
        }
    }
}