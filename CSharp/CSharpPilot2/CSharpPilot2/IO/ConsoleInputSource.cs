using System;
using System.Timers;

namespace CSharpPilot2.IO {
    sealed class ConsoleInputSource : IInputSource {
        public bool Intercept { get; set; } = false;

        public Input Get() {
            using Timer timer = new(100.0);
            double seconds = 0.0;
            timer.Elapsed += (_, _) => seconds += 0.1;
            
            timer.Start();
            string text = Read() ?? "";
            timer.Stop();

            return new(text, seconds);
        }

        private string? Read() {
            if (Intercept) {
                return $"{Console.ReadKey(intercept: true)}";
            } else {
                return Console.ReadLine();
            }
        }
    }
}
