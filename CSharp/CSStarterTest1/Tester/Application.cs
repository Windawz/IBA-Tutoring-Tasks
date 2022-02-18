using System;
using System.CodeDom.Compiler;
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
    internal partial class Application : IDisposable
    {
        public Application(string[] testedAssemblies)
        {
            _testedAssemblies = testedAssemblies
                .Select(s => new AssemblyName(s.Trim()))
                .ToArray();
        
            var writer = GetIndentedTextWriter();
            _override = new ConsoleOutputOverride(writer);
            _indentControl = new IndentControl(writer);
            
            Console.ForegroundColor = ConsoleColor.White;
        }

        public int ExitCode { get; private set; }

        private AssemblyName[] _testedAssemblies;
        private ConsoleOutputOverride _override;
        private IndentControl _indentControl;
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

            var processor = new StageProcessor<AssemblyName, Nothing>(_indentControl, stages);
            _ = processor.Process(_testedAssemblies);

            ExitCode = 0;
        }
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _override.Dispose();

            try
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(intercept: true);
            }
            catch (Exception) { }

            _disposed = true;
        }

        private static IndentedTextWriter GetIndentedTextWriter()
        {
            return new IndentedTextWriter(ConsoleOutput.Out.GetWriter(), " ");
        }
    }
}
