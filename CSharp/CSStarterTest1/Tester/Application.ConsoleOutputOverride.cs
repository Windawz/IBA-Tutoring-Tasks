using System;
using System.CodeDom.Compiler;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal partial class Application
    {
        private class ConsoleOutputOverride : IDisposable
        {
            public ConsoleOutputOverride(IndentedTextWriter outWriter)
            {
                _oldOut = ConsoleOutput.Out.SetWriter(outWriter);
                _oldError = ConsoleOutput.Error.SetWriter(GetErrorLogger());
            }

            private static readonly LogFileNameProvider _nameProvider = new();
            private static readonly LoggerProvider _loggerProvider = new();
            private readonly SavedConsoleOutput _oldOut;
            private readonly SavedConsoleOutput _oldError;
            private bool _disposed;

            public void Dispose()
            {
                if (!_disposed)
                {
                    _oldOut.Dispose();
                    _oldError.Dispose();
                }
                _disposed = true;
            }

            private static TextWriter GetErrorLogger()
            {
                TextWriter logger;
                try
                {
                    string fileName = _nameProvider.GetName("Tester");
                    logger = _loggerProvider.GetLogger(fileName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get console output override logger; exception: {ex}");
                    return TextWriter.Null;
                }
                return logger;
            }
        }
    }
}
