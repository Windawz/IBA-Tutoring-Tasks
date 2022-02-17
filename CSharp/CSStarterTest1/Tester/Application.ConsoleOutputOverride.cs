﻿using System;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal partial class Application
    {
        private class ConsoleOutputOverride : IDisposable
        {
            public ConsoleOutputOverride()
            {
                _errorSaver = ConsoleOutput.Error.SetWriter(GetErrorLogger());
            }

            private static readonly LogFileNameProvider _nameProvider = new();
            private static readonly LoggerProvider _loggerProvider = new();
            private readonly SavedConsoleOutput _errorSaver;

            public void Dispose() => _errorSaver.Dispose();

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
