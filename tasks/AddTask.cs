using System;
using cit.secret_service;
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
                var isSecure = false;
                var itemValue = command.ItemValue;
                if(command.Password != null)
                {
                    isSecure = true;
                    itemValue = AESService.EncryptString(itemValue, command.Password, command.Salt);
                }
                Store.Add(command.EnvName, command.KeyName, itemValue, isSecure);
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