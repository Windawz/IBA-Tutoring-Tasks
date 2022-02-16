using System;
using System.CodeDom.Compiler;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal class ConsoleIndenter : IDisposable
    {
        public ConsoleIndenter(ConsoleOutput outputKind)
        {
            if (!Enum.IsDefined(outputKind))
            {
                throw new ArgumentOutOfRangeException(nameof(outputKind));
            }

            _outputKind = outputKind;

            _old = GetConsoleOutput(_outputKind);
            _indented = new IndentedTextWriter(_old, " ");

            SetConsoleOutput(_outputKind, _indented);
        }

        private ConsoleOutput _outputKind;
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
                SetConsoleOutput(_outputKind, _old);
                _disposed = true;
            }
        }

        private static TextWriter GetConsoleOutput(ConsoleOutput outputKind) => outputKind switch
        {
            ConsoleOutput.Out => Console.Out,
            ConsoleOutput.Error => Console.Error,
            _ => throw new InvalidOperationException($"Invalid {nameof(_outputKind)} value"),
        };
        private static void SetConsoleOutput(ConsoleOutput outputKind, TextWriter writer)
        {
            switch (outputKind)
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
