using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpPilot2.Commands;
using CSharpPilot2.LegacyGameplay;
using CSharpPilot2.IO;
using CSharpPilot2.Locales;

namespace CSharpPilot2
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Locale locale = new RussianLocale();

            Encoding encoding = locale.GetEncoding();
            Console.InputEncoding = encoding;
            Console.OutputEncoding = encoding;

            Game game = new(new ConsoleInputSource(), GetRules(), locale, GetDefaultCommandOptions());
            game.Start();
        }

        static Rules GetRules()
        {
            RulesProperties properties = new(2, 10.0, 8, 30);

            return new Rules(properties, GetDefaultWordValidator(properties), GetDefaultInputValidator(properties));
        }
        static WordValidator GetDefaultWordValidator(RulesProperties properties) =>
            (cur, prev) =>
            {
                if (String.Equals(cur.Text, prev.Text, StringComparison.InvariantCultureIgnoreCase)
                    || (cur.Seconds > properties.MaxWordSeconds)
                )
                {
                    return false;
                }

                IEnumerable<(char Char, int Count)>? curCounts = cur.Text.ToLowerInvariant().CharacterCounts();
                IEnumerable<(char Char, int Count)>? prevCounts = prev.Text.ToLowerInvariant().CharacterCounts();

                return curCounts.SequenceEqual(prevCounts);
            };
        static InputValidator GetDefaultInputValidator(RulesProperties properties) =>
            (inputInfo) => inputInfo.Text.Length <= properties.MaxWordTextLength
                           && inputInfo.Text.Length >= properties.MinWordTextLength;
        static CommandOptions GetDefaultCommandOptions() =>
            new CommandOptions("/", "-", new string[] { " " }, GetDefaultCommandList());
        static CommandList GetDefaultCommandList()
        {
            Dictionary<string, CommandInfo> d = new();
            // No functionality for those to work yet.
            //{
            //    { "show-words",     new("Show words", "Shows all words entered throughout the game by the players.")    },
            //    { "score",          new("Score", "Shows the total score from all games for current players.")           },
            //    { "total-score",    new("Total score", "Shows the total score from all games for all players.")         }
            //};
            return new CommandList(d);
        }
    }
}
