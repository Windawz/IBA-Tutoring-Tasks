using CSharpPilot2.Input;

namespace CSharpPilot2.Gameplay
{
    internal class Rules
    {
        public Rules(RulesProperties properties, WordValidator wordValidator, InputValidator inputValidator, InputForgiver inputForgiver)
        {
            Properties = properties;
            WordValidator = wordValidator;
            InputValidator = inputValidator;
            InputForgiver = inputForgiver;
        }

        public RulesProperties Properties { get; }
        public WordValidator WordValidator { get; }
        public InputValidator InputValidator { get; }
        public InputForgiver InputForgiver { get; }
    }
}
