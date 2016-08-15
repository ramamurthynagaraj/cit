using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using cit.utilities;

namespace cit.tasks
{
    class ApplyTask : ITask
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
            var applyArguments = commands.Skip(1).ToList();
            var fileFlagIndex = applyArguments.FindIndex(arg => "-f".Equals(arg));
            var files = applyArguments.Skip(fileFlagIndex + 1).ToList();
            
            var environments = applyArguments.Take(fileFlagIndex).ToList();
            var allEnvironmentsValid = environments.All(Store.IsEnvironmentExists);
            if(!allEnvironmentsValid)
            {
                _logger($"Some environments are missing. Please provide the correct environments.");
                return 1;
            }
            var finalValues = Store.GetFinalValuesFor(environments);
            try{
                files.ForEach(filePath => ReplaceTemplate(filePath, finalValues));
            }
            catch(TemplateFileNotFoundException ex)
            {
                _logger($"{ex.Message}");
                return 1;
            }
            _logger("Applied successfully");
            return 0;
        }

        private void ReplaceTemplate(string filePath, Dictionary<string, string> values)
        {
            if(!File.Exists(filePath))
            {
                throw new TemplateFileNotFoundException($"{filePath} not found. Please verify if it exists.");
            }
            var fileContent = File.ReadAllText(filePath);
            var newFileContent = values.Keys.ToList().Aggregate(fileContent, (content, key) => {
                return content.Replace($"#{key}", values[key]);
            });
            File.WriteAllText(filePath, newFileContent);
            _logger($"Updated {filePath}");            
        }
    }

    public class TemplateFileNotFoundException : Exception {

        public TemplateFileNotFoundException(string message) : base(message)
        {
        }
    }
}