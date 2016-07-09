using System;
using System.Linq;
using cit.utilities;

namespace cit.tasks
{
    public class ApplyTask : ITask
    {
        Action<string> _logger;

        public ApplyTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            if(commands.Length == 1){
                _logger("Environments missing, mention the environment(s) that needs to be applied. E.g 'cit apply staging', 'cit apply testing staging'");
                return 1;
            }
            var environments = commands.ToList().Skip(1).ToList();
            var allEnvironmentsValid = environments.All(Store.IsEnvironmentExists);
            if(!allEnvironmentsValid)
            {
                _logger($"Some environments are missing. Please provide the correct environments.");
                return 1;
            }
            var finalValues = Store.GetFinalValuesFor(environments);
            _logger("The final values are");
            finalValues.Keys.ToList().ForEach(key => {
                _logger($"\t{key}: {finalValues[key]}");
            });
            return 0;
        }
    }
}