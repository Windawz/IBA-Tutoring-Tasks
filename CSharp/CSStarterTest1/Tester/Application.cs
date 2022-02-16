using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using CSStarterTest1.Tester.Stages;
using CSStarterTest1.Tester.Stages.ConcreteStages;
using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal class Application : IDisposable
    {
        public Application(string[] testedAssemblies)
        {
            _errorWriter = new LoggerProvider().GetLogger(
                new LogFileNameProvider().GetName("Tester")
            );
            Console.SetError(_errorWriter);

            _testedAssemblies = testedAssemblies
                .Select(s => new AssemblyName(s.Trim()))
                .ToArray();

            _indenter = SimpleConsoleIndenter.GetIndenter(ConsoleOutput.Out);
            _indenter.IndentStep = 4;

            Console.ForegroundColor = ConsoleColor.White;
        }

        public int ExitCode { get; private set; }

        private TextWriter _errorWriter;
        private AssemblyName[] _testedAssemblies;
        private SimpleConsoleIndenter _indenter;
        private bool _disposed;

        public void Run()
        {
            IStage[] stages =
            {
                new LoadTestableAssembliesStage(),
                new GetTestTypesStage(),
                new InstantiateTestsStage(),
                new PerformTestsAndGetStatusStage(),
            };

            var processor = new StageProcessor<AssemblyName, Nothing>(_indenter, stages);
            _ = processor.Process(_testedAssemblies);

            ExitCode = 0;
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    _errorWriter.Dispose();
                    _indenter.Dispose();

                    try
                    {
                        Console.WriteLine();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(intercept: true);
                    }
                    catch (Exception) { }
                }

                // Free unmanaged resources (unmanaged objects) and override finalizer
                // Set large fields to null
                _disposed = true;
            }
        }

        // Left in case Application has to handle unmanaged resources
        //
        // // Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Application()
        // {
        //     Dispose(disposing: false);
        // }
    }
}
