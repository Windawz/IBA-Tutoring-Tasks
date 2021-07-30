﻿using System;
using System.Timers;

using CSharpPilot2.Input;
using CSharpPilot2.Gameplay;

namespace CSharpPilot2
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            Game game = new(
                inputSource:    ReadInputInfo,
                rules:          GetRules(),
                locale:         new RussianLocale()
            );

            game.Start();

            Console.WriteLine();
            foreach (Step step in game.Steps)
            {
                Console.WriteLine("{0}", step);
            }
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

            return new Rules(
                properties,
                Rules.GetDefaultWordValidator(properties),
                Rules.GetDefaultInputValidator(properties),
                inputInfo => Command.IsValidCommandName(inputInfo.Text)
            );
        }
    }
}
