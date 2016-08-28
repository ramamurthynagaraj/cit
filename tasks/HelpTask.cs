using System;

namespace cit.tasks
{
    class HelpTask
    {
        Action<string> _logger;
        public HelpTask(Action<string> logger)
        {
            _logger = logger;
        }

        public int HandleCommand(string[] commands)
        {
            _logger("Provided command not found.");
            return 0;
        }
    }
}