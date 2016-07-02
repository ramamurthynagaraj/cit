using System;

using cit.utilities;
using cit.utilities.validators;

namespace cit.tasks
{
    class CopyTask : ITask
    {
        Action<string> _logger;
        public CopyTask(Action<string> logger)
        {
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            if (commands.Length != 3)
            {
                _logger("Wrong number of parameters specified");
                return 1;
            }
            return CopyEnvironment(commands[1], commands[2]);
        }

        internal int CopyEnvironment(string fromEnv, string envName)
        {
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
            Store.Copy(fromEnv, envName);
            _logger($"Environment: {envName} created succesfully from {fromEnv}.");
            return 0;
        }
    }
}