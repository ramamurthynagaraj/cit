using System.Collections.Generic;
namespace cit.tasks.commands
{
    public class ApplyCommand
    {
        public List<string> Environments {get; set;}
        public List<string> Files {get; set;}
    }
}