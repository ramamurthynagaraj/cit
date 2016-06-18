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
            if(commands.Length != 4){
                _logger("Parameters missig, mention all parameters. E.g 'cit staging add key value', 'cit staging add key \"value with spaces\"'");
                return 1;
            }
            return 0;
        }

    }
}