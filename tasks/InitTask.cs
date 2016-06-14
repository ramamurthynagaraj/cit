using System;
using System.IO;

using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class InitTask
    {
        Action<string> _logger;
        public InitTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(string[] commands){
            var envName = Constants.DefaultEnvName;
            if (commands.Length == 2)
            {
                envName = commands[1];
            }
            if (Validators.FileNameValidators.IsMatch(envName))
            {
                var fileName = $"{envName}.json";
                if (File.Exists(fileName))
                {
                    _logger($"Environment: {envName} already exists. Not creating it again.");
                    return 1;
                }
                File.Create(fileName);
                _logger($"Environment: {envName} created succesfully.");
            }
            else
            {
                _logger("Environment name should contain alphanumeric characters. E.g staging, staging01, staging_1");
                return 1;
            }
            return 0;
        }
    }
}