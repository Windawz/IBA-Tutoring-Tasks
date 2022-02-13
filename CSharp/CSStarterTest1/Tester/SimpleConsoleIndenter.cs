
using System;

namespace CSStarterTest1.Tester
{
    internal class SimpleConsoleIndenter : IDisposable
    {
        public const int DefaultIndentStep = 2;

        public SimpleConsoleIndenter(ConsoleIndenter indenter, bool disposeIndenter = true)
        {
            InnerIndenter = indenter;
            _disposeIndenter = disposeIndenter;
        }

        private bool _disposeIndenter;
        private bool _disposed;

        public static SimpleConsoleIndenter GetIndenter(ConsoleOutputKind outputKind) => 
            new SimpleConsoleIndenter(new ConsoleIndenter(outputKind));
        public ConsoleIndenter InnerIndenter { get; }
        public int IndentStep { get; set; } = DefaultIndentStep;
        
        public void Increase() => InnerIndenter.Indent += IndentStep;
        public void Decrease() => InnerIndenter.Indent -= IndentStep;
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_disposeIndenter)
                    {
                        InnerIndenter.Dispose();
                    }
                }
                _disposed = true;
            }
        }
    }
}
