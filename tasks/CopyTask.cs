using System;
using cit.tasks.commands;
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
            var command = Parse(commands);
            if(command == null)
            {
                return 1;
            }

            return CopyEnvironment(command.FromEnvName, command.ToEnvName);
        }

        private CopyCommand Parse(string[] commands)
        {
            if (commands.Length != 3)
            {
                _logger("Wrong number of parameters specified");
                return null;
            }
            return new CopyCommand {
                FromEnvName = commands[0],
                ToEnvName = commands[1]
            };
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