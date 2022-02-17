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

            _writer = new IndentedTextWriter(consoleOutput.GetWriter(), " ");
            _savedOutput = consoleOutput.SetWriter(_writer);
        }

        private SavedConsoleOutput _savedOutput;
        private IndentedTextWriter _writer;
        private bool _disposed;

        public int Indent
        {
            get => _writer.Indent;
            set => _writer.Indent = value;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _savedOutput.Dispose();
            }
            _disposed = true;
        }
    }
}
