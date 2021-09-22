using System;
using System.Text;
using System.Timers;

namespace CSharpPilot2.IO
{
    sealed class ConsoleInputSource : IInputSource
    {
        public ConsoleInputSource(Encoding encoding) =>
            _encoding = encoding;
            
        private readonly Encoding _encoding;

        public InputSourceReadMode ReadMode { get; set; }

        public Input Get()
        {
            Encoding oldEncoding = Console.InputEncoding;
            Console.InputEncoding = _encoding;

            using Timer timer = new(100.0);
            double seconds = 0.0;
            timer.Elapsed += (_, _) => seconds += 0.1;

            timer.Start();
            string text = Read();
            timer.Stop();

            Console.InputEncoding = oldEncoding;

            return new(text, seconds);
        }

        string Read()
        {
            switch (ReadMode)
            {
                default:
                case InputSourceReadMode.Line:
                    return Console.ReadLine() ?? "";
                case InputSourceReadMode.Key:
                    return $"{Console.ReadKey(intercept: true)}";
                case InputSourceReadMode.Skip:
                    return "";
            }
        }
    }
}
