using System.Text.RegularExpressions;

namespace cit.utilities.validators
{
    static class Validators
    {
        public static Regex FileNameValidators { get; } = new Regex("[a-z_0-9]+");
    }
}