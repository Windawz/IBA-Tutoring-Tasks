using System;
using System.IO;

namespace CSStarterTest1.Tester
{
    /// <summary>
    /// Takes the specified console output, copies the writer and keeps it.
    /// When disposed, assigns the writer back.
    /// </summary>
    internal class SavedConsoleOutput : IDisposable
    {
        /// <param name="output">Console output to save.</param>
        public SavedConsoleOutput(ConsoleOutput output)
        {
            _output = output;
            _old = output.GetWriter();
        }
        
        private readonly TextWriter _old;
        private readonly ConsoleOutput _output;
        private bool _disposed;

        public void Restore() => Dispose();
        public void Dispose()
        {
            if (!_disposed)
            {
                _output.SetWriter(_old);
                _disposed = true;
            }
        }
    }
}
