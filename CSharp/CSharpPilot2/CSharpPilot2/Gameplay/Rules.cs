using System;
using System.Linq;

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

        public static WordValidator GetDefaultWordValidator(RulesProperties properties) =>
            (cur, prev) =>
            {
                if (String.Equals(cur.Text, prev.Text, StringComparison.InvariantCultureIgnoreCase)
                    || (cur.Seconds > properties.MaxWordSeconds)
                )
                {
                    return false;
                }

                System.Collections.Generic.IEnumerable<(char Char, int Count)>? curCounts = cur.Text.ToLowerInvariant().CharacterCounts();
                System.Collections.Generic.IEnumerable<(char Char, int Count)>? prevCounts = prev.Text.ToLowerInvariant().CharacterCounts();

                return curCounts.SequenceEqual(prevCounts);
            };
        public static InputValidator GetDefaultInputValidator(RulesProperties properties) =>
            (inputInfo) => inputInfo.Text.Length <= properties.MaxWordTextLength
                           && inputInfo.Text.Length >= properties.MinWordTextLength;
    }
}
