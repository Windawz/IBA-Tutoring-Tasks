using System;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal static class ConsoleOutputExtensions
    {
        /// <summary>
        /// Returns the corresponding standard output stream of the console.
        /// </summary>
        public static TextWriter GetWriter(this ConsoleOutput consoleOutput) => consoleOutput switch
        {
            ConsoleOutput.Out => Console.Out,
            ConsoleOutput.Error => Console.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(consoleOutput)),
        };
        /// <summary>
        /// Sets the corresponding standard output stream of the console.
        /// </summary>
        public static SavedConsoleOutput SetWriter(this ConsoleOutput consoleOutput, TextWriter newWriter)
        {
            var saver = new SavedConsoleOutput(consoleOutput);
            SetWriterImpl(consoleOutput, newWriter);
            return saver;
        }

        private static void SetWriterImpl(ConsoleOutput consoleOutput, TextWriter newWriter)
        {
            switch (consoleOutput)
            {
                case ConsoleOutput.Out:
                    Console.SetOut(newWriter);
                    break;
                case ConsoleOutput.Error:
                    Console.SetError(newWriter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(consoleOutput));
            }
        }
    }
}
