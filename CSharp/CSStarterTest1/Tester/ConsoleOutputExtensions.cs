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
        public static void SetWriter(this ConsoleOutput consoleOutput, TextWriter newWriter)
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
