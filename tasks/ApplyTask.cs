using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using cit.utilities;
using cit.tasks.commands;

namespace cit.tasks
{
    class ApplyTask
    {
        Action<string> _logger;

        public ApplyTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(ApplyCommand command)
        {
            if(command == null)
            {
                return 1;
            }
            if(command.Environments.Count == 0)
            {
                _logger($"No environments provided.");
                return 1;
            }
            if(command.Files.Count == 0)
            {
                _logger($"No files provided.");
                return 1;
            }
            var allEnvironmentsValid = command.Environments.All(Store.IsEnvironmentExists);
            if(!allEnvironmentsValid)
            {
                _logger($"Some environments are missing. Please provide the correct environments.");
                return 1;
            }

            var finalValues = Store.GetFinalValuesFor(command.Environments);
            try{
                command.Files.ForEach(filePath => ReplaceTemplate(filePath, finalValues));
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