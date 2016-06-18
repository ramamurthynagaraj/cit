using System.Text.RegularExpressions;

namespace cit.utilities.validators
{
    static class Validators
    {
        public static Regex EnvNameValidator { get; } = new Regex("[a-z_0-9]+");
        public static Regex KeyNameValidator { get; } = new Regex("[a-z_0-9]+");
    }
}