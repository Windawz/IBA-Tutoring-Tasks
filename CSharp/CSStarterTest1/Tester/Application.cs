using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal class Application : IDisposable
    {
        private const int DefaultIndentStep = 2;

        public Application(string[] testedAssemblies)
        {
            _errorWriter = new StreamWriter(File.OpenWrite("TesterLog.log"), Encoding.UTF8, leaveOpen: false);
            Console.SetError(_errorWriter);

            _testedAssemblies = testedAssemblies
                .Select(s => new AssemblyName(s.Trim()))
                .ToArray();

            _indenter = SimpleConsoleIndenter.GetIndenter(ConsoleOutputKind.Out);

            Console.ForegroundColor = ConsoleColor.White;
        }

        public int ExitCode { get; private set; }

        private StreamWriter _errorWriter;
        private AssemblyName[] _testedAssemblies;
        private SimpleConsoleIndenter _indenter;
        private bool _disposed;

        public void Run()
        {
            Assembly[] referencedAssemblies = LoadAndPrintTestableAssemblies();
            Console.WriteLine();

            Type[] testTypes = GetAndPrintTestTypesFrom(referencedAssemblies);
            Console.WriteLine();

            Test[] tests = InstantiateTestsAndReport(testTypes);
            Console.WriteLine();
            
            PerformTestsAndReport(tests);
            
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

        private void PerformTestsAndReport(IEnumerable<Test> tests)
        {
            Console.WriteLine("Performing tests...");

            Dictionary<Test, TestResult> results = TestPerformer.Perform(tests);

            Console.WriteLine("Test results:");
            PrintResults(results);
        }
        private Test[] InstantiateTestsAndReport(IEnumerable<Type> testTypes)
        {
            Console.WriteLine("Instantiating tests...");

            var tests = new List<Test>();
            var loggerProvider = new LoggerProvider();
            var nameGen = new TestLogNameGenerator();

            Console.WriteLine("Instantiated tests:");
            _indenter.Increase();
            foreach (Type testType in testTypes)
            {
                string testName = testType.Name;

                TextWriter logger;
                try
                {
                    logger = loggerProvider.GetLogger(nameGen.GetLogName(testName));
                }
                catch (Exception ex)
                {
                    string message = $"Failed to create log file for \"{testName}\"";
                    WriteLineColored(message, ConsoleColor.Yellow);
                    // More detailed info goes into the error stream.
                    Console.Error.WriteLine($"{message}: \"{ex}\"");
                    logger = TextWriter.Null;
                }

                Test test;
                try
                {
                    test = TestInstantiator.Instantiate(testType, logger);
                }
                catch (Exception ex)
                {
                    WriteLineColored($"{testType.Name} ({ex.GetType().Name}) : {ex.Message}", ConsoleColor.Red);

                    // Failed to instantiate test, no need for the logger
                    logger.Dispose();

                    continue;
                }
                WriteLineColored(testType.Name, ConsoleColor.Green);
                tests.Add(test);
            }
            _indenter.Decrease();

            return tests.ToArray();
        }
        private Type[] GetAndPrintTestTypesFrom(IEnumerable<Assembly> assemblies)
        {
            Console.WriteLine("Getting test types...");

            Type[] testTypes = assemblies
                .SelectMany(a => TestFinder.LoadTestTypes(a))
                .ToArray();

            Console.WriteLine("Test types:");

            _indenter.Increase();
            foreach (Type testType in testTypes)
            {
                WriteLineColored(testType.Name, ConsoleColor.Green);
            }
            _indenter.Decrease();

            return testTypes;
        }
        private Assembly[] LoadAndPrintTestableAssemblies()
        {
            Console.WriteLine("Loading testable assemblies...");

            AssemblyLoadInfo[] infos = _testedAssemblies.Select(name => AssemblyLoader.Load(name)).ToArray();

            Console.WriteLine("Loaded assemblies:");

            _indenter.Increase();
            foreach (var info in infos)
            {
                WriteLineColored($"{info.Name.Name}", info.LoadedAssembly is null ? ConsoleColor.Red : ConsoleColor.Green);
            }
            _indenter.Decrease();

            return infos
                .Where(i => i.LoadedAssembly is not null)
                .Select(i => i.LoadedAssembly!)
                .ToArray();
        }
        private void PrintResults(Dictionary<Test, TestResult> results)
        {
            var sorted = results.OrderByDescending(kv => kv.Value);

            foreach (var result in sorted)
            {
                WriteLineColored($"  {result.Key.GetType().Name}", result.Value.GetConsoleColor());
            }
        }
        private void WriteLineColored(string text, ConsoleColor color)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }
    }
}
