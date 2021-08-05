using System;
using System.Timers;
using System.Linq;

using CSharpPilot2.Gameplay;
using CSharpPilot2.Input;
using System.Text;

using CSharpPilot2.Locales;

namespace CSharpPilot2
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            Locale locale = new RussianLocale();

            Encoding encoding = locale.GetEncoding();
            Console.InputEncoding = encoding;
            Console.OutputEncoding = encoding;

            Game game = new(ReadInputInfo, GetRules(), locale);
            game.Start();
        }

        private static InputInfo ReadInputInfo()
        {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string? text = Console.ReadLine() ?? "";
            timer.Stop();

            var inputInfo = new Input.InputInfo(text, time);

            return inputInfo;
        }
        private static Rules GetRules()
        {
            RulesProperties properties = new(2, 10.0, 8, 30);

            return new Rules(properties, GetDefaultWordValidator(properties), GetDefaultInputValidator(properties),
                inputInfo => false);
        }
        private static WordValidator GetDefaultWordValidator(RulesProperties properties) =>
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
        private static InputValidator GetDefaultInputValidator(RulesProperties properties) =>
            (inputInfo) => inputInfo.Text.Length <= properties.MaxWordTextLength
                           && inputInfo.Text.Length >= properties.MinWordTextLength;
    }
}
