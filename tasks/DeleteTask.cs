using System;
using System.IO;

using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class DeleteTask
    {
        Action<string> _logger;
        public DeleteTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(string[] commands){
            var envName = string.Empty;
            if (commands.Length == 2)
            {
                envName = commands[1];
            }
            if (Validators.FileNameValidators.IsMatch(envName))
            {
                var fileName = $"{envName}.json";
                if (!File.Exists(fileName))
                {
                    _logger($"Environment: {envName} does not exists. Please verify the supplied environment name.");
                    return 1;
                }
                File.Delete(fileName);
                _logger($"Environment: {envName} deleted successfully.");
            }
            else
            {
                _logger("Environment name cannot be empty and should contain alphanumeric characters. E.g staging, staging01, staging_1");
                return 1;
            }
            return 0;
        }
    }
}