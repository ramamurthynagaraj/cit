using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using cit.utilities;
using cit.tasks.commands;
using cit.secret_service;

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

            var normalValues = Store.GetFinalValuesFor(command.Environments, false);
            var secureValues = Store.GetFinalValuesFor(command.Environments, true).Select(pair => {
                var decryptedValue = AESService.DecryptString(pair.Value, command.Password, command.Salt);
                return new KeyValuePair<string, string>(pair.Key, decryptedValue);
            }).ToDictionary(pair => pair.Key, pair => pair.Value);
            var finalValues = normalValues.Union(secureValues).ToDictionary(pair => pair.Key, pair => pair.Value);

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
            var newFileContent = values.Keys.ToList().OrderByDescending(key => key.Length).Aggregate(fileContent, (content, key) => {
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