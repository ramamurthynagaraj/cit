using System;

namespace cit.tasks
{
    public class AddTask : ITask
    {
        Action<string> _logger;

        public AddTask(Action<string> logger){
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            if(commands.Length != 3){
                _logger("Add command should have both key and value. E.g 'cit add key value', 'cit add key \"value with spaces\"'");
                return 1;
            }
            return 0;
        }
    }
}