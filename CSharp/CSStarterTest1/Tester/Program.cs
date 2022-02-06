using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    public sealed class Program
    {
        private static readonly Dictionary<TestResult, ConsoleColor> _testResultColors = new()
        {
            [TestResult.Success] = ConsoleColor.Green,
            [TestResult.Failure] = ConsoleColor.Red,
        };
        private static readonly AssemblyName[] _testedAssemblyNames =
        {
            new AssemblyName("DatabaseInterface"),
            new AssemblyName("DataOps"),
        };
        private static readonly TextWriter _testLogWriter = TryGetLogWriter("tests.log") ?? TextWriter.Null;

        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (_testLogWriter == TextWriter.Null)
            {
                WriteLineColored("Failed to create a log file for tests.", ConsoleColor.Yellow);
                Console.WriteLine();
            }

            Assembly[] referencedAssemblies = LoadAndPrintTestableAssemblies();
            Console.WriteLine();

            Type[] testTypes = GetAndPrintTestTypesFrom(referencedAssemblies);
            Console.WriteLine();

            Test[] tests = InstantiateTestsAndReport(testTypes);
            Console.WriteLine();

            PerformTestsAndReport(tests);

            Finish(0);
        }
        private static void PerformTestsAndReport(IEnumerable<Test> tests)
        {
            Console.WriteLine("Performing tests...");

            Dictionary<Test, TestResult> results = TestPerformer.Perform(tests);

            Console.WriteLine("Test results:");
            PrintResults(results);
        }
        private static Test[] InstantiateTestsAndReport(IEnumerable<Type> testTypes)
        {
            Console.WriteLine("Instantiating tests...");

            List<Test> tests = new();

            Console.WriteLine("Instantiated tests:");
            foreach (Type testType in testTypes)
            {
                Test test;
                try
                {
                    test = TestInstantiator.Instantiate(testType, _testLogWriter);
                }
                catch (Exception ex)
                {
                    WriteLineColored($"  {testType.Name} ({ex.GetType().Name}) : {ex.Message}", ConsoleColor.Red);
                    continue;
                }
                WriteLineColored($"  {testType.Name}", ConsoleColor.Green);
                tests.Add(test);
            }

            return tests.ToArray();
        }
        private static Type[] GetAndPrintTestTypesFrom(IEnumerable<Assembly> assemblies)
        {
            Console.WriteLine("Getting test types...");

            Type[] testTypes = assemblies
                .SelectMany(a => TestFinder.LoadTestTypes(a))
                .ToArray();

            Console.WriteLine("Test types:");
            foreach (Type testType in testTypes)
            {
                WriteLineColored($"  {testType.Name}", ConsoleColor.Green);
            }

            return testTypes;
        }
        private static Assembly[] LoadAndPrintTestableAssemblies()
        {
            Console.WriteLine("Loading testable assemblies...");

            AssemblyLoadInfo[] infos = _testedAssemblyNames.Select(name => AssemblyLoader.Load(name)).ToArray();

            Console.WriteLine("Loaded assemblies:");
            foreach (var info in infos)
            {
                WriteLineColored($"  {info.Name.Name}", info.LoadedAssembly is null ? ConsoleColor.Red : ConsoleColor.Green);
            }

            return infos
                .Where(i => i.LoadedAssembly is not null)
                .Select(i => i.LoadedAssembly!)
                .ToArray();
        }
        private static void PrintResults(Dictionary<Test, TestResult> results)
        {
            var sorted = results.OrderByDescending(kv => kv.Value);

            foreach (var result in sorted)
            {
                WriteLineColored($"  {result.Key.GetType().Name}", _testResultColors[result.Value]);
            }
        }
        private static void WriteLineColored(string text, ConsoleColor color)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }
        private static TextWriter? TryGetLogWriter(string fileName)
        {
            Stream? stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            catch (SystemException)
            {
                if (stream is not null)
                {
                    stream.Dispose();
                }
            }
            if (stream is not null)
            {
                return new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: false);
            }
            else
            {
                return null;
            }
        }
        private static void Finish(int exitCode)
        {
            _testLogWriter.Close();    

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(intercept: true);
            Environment.Exit(exitCode);
        }
    }
}
