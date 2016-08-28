using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using cit.utilities;
using cit.tasks.commands;

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
            var command = Parse(commands);
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

        private ApplyCommand Parse(string[] commands)
        {
            if(commands.Length == 1){
                _logger("Parameters missing, mention the environment(s) that needs to be applied to file(s). E.g 'cit apply staging -f file1', 'cit apply testing staging -f file1'");
                return null;
            }
            var applyArguments = commands.Skip(1).ToList();
            var environments = applyArguments.TakeWhile(arg => !"-f".Equals(arg)).ToList(); 
            
            var files = new List<string>();           
            var fileFlagIndex = applyArguments.FindIndex(arg => "-f".Equals(arg));
            if(fileFlagIndex  != -1)
            {
                files = applyArguments.Skip(fileFlagIndex + 1).ToList();      
            }

            return new ApplyCommand {
                Files = files,
                Environments = environments
            };
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