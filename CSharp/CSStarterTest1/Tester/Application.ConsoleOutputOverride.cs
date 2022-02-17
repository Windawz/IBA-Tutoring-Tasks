using System;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal partial class Application
    {
        private class ConsoleOutputOverride : IDisposable
        {
            private static readonly int ConsoleOutputCount = Enum.GetValues<ConsoleOutput>().Length;

            public ConsoleOutputOverride()
            {
                _errorSaver = ConsoleOutput.Error.SetWriter(GetErrorLogger());
            }

            private static readonly LogFileNameProvider _nameProvider = new();
            private static readonly LoggerProvider _loggerProvider = new();
            private readonly ConsoleOutputSaver _errorSaver;

            public void Dispose() => _errorSaver.Dispose();

            private static TextWriter GetLogger(string name)
            {
                TextWriter logger;
                try
                {
                    string fileName = _nameProvider.GetName(name);
                    logger = _loggerProvider.GetLogger(fileName);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get console output override logger; exception: {ex}");
                    return TextWriter.Null;
                }
                return logger;
            }
            private TextWriter GetErrorLogger()
            {
                return GetLogger("Tester");
            }
        }
    }
}
