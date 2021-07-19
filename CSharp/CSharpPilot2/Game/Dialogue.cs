using System;
using System.Timers;

namespace Game {
    static class Dialogue {
        // Reads user input, just like Console.ReadLine().
        // The input is validated using the passed function.
        // The validation result will be returned along with the rest of the input.
        public static InputInfo ReadLineValidated(Func<string, bool> textValidator) {
            double time = 0.0;

            var timer = new Timer(100);
            timer.Elapsed += (sender, e) => time += 0.1;

            timer.Start();
            string text = Console.ReadLine() ?? "";
            timer.Stop();

            return new InputInfo(text, time, textValidator(text));
        }
    }
}
