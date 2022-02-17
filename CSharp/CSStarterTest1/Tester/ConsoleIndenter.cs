using System;
using System.CodeDom.Compiler;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal class ConsoleIndenter : IDisposable
    {
        public ConsoleIndenter(ConsoleOutput consoleOutput)
        {
            if (!Enum.IsDefined(consoleOutput))
            {
                throw new ArgumentOutOfRangeException(nameof(consoleOutput));
            }

            _consoleOutput = consoleOutput;

            _old = GetConsoleOutput(_consoleOutput);
            _indented = new IndentedTextWriter(_old, " ");

            SetConsoleOutput(_consoleOutput, _indented);
        }

        private ConsoleOutput _consoleOutput;
        private TextWriter _old;
        private IndentedTextWriter _indented;
        private bool _disposed;

        public int Indent
        {
            get => _indented.Indent;
            set => _indented.Indent = value;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                SetConsoleOutput(_consoleOutput, _old);
                _disposed = true;
            }
        }

        private static TextWriter GetConsoleOutput(ConsoleOutput consoleOutput) => consoleOutput switch
        {
            ConsoleOutput.Out => Console.Out,
            ConsoleOutput.Error => Console.Error,
            _ => throw new InvalidOperationException($"Invalid {nameof(_consoleOutput)} value"),
        };
        private static void SetConsoleOutput(ConsoleOutput consoleOutput, TextWriter writer)
        {
            switch (consoleOutput)
            {
                case ConsoleOutput.Out:
                    Console.SetOut(writer);
                    break;
                case ConsoleOutput.Error:
                    Console.SetError(writer);
                    break;
            }
        }

        ~ConsoleIndenter()
        {
            Dispose(disposing: false);
        }
    }
}
