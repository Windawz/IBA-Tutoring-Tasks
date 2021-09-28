using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace CSStarterTest1.Tester
{
    public class Program
    {
        public static void Main()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Assembly assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Executing assembly: \"{assembly.GetName()}\"");

            TextWriter? logWriter = TryGetLogWriter("tests.log");
            if (logWriter is null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Failed to create a log file for tests.");
                Console.ForegroundColor = defaultColor;
                logWriter = TextWriter.Null;
            }

            Test[] tests = null!;
            try
            { 
                tests = GatherTests(assembly, logWriter);
            }
            catch (InvalidOperationException e)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Failed to gather tests. Error message: \"{e.Message}\"");
                Console.ForegroundColor = defaultColor;
                Die(-1);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine($"{tests.Length} test(s) detected:");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (Test t in tests)
            {
                Console.WriteLine($" + {t.GetType().Name}");
            }
            Console.ForegroundColor = defaultColor;

            Console.WriteLine();
            Console.WriteLine($"Performing...");
            Console.WriteLine();

            Dictionary<Test, TestResult> results = new();
            foreach (Test t in tests)
            {
                results[t] = t.Perform();
            }

            logWriter.Close();

            Console.ForegroundColor = ConsoleColor.Green;
            PrintResults(results, TestResult.Success);

            Console.ForegroundColor = ConsoleColor.Red;
            PrintResults(results, TestResult.Failure);

            Console.ForegroundColor = defaultColor;

            Die(0);
        }

        private static Test[] GatherTests(Assembly assembly, TextWriter logWriter)
        {
            var types = assembly
                .GetTypes()
                .Where(t => t.BaseType == typeof(Test));

            List<Test> tests = new();
            foreach (Type t in types)
            {
                ConstructorInfo? ctor = t.GetConstructor(new Type[] { typeof(TextWriter) });
                if (ctor is null)
                {
                    throw new InvalidOperationException($"Test \"{t.Name}\" has no fitting constructor");
                }

                Test test = (Test)ctor.Invoke(new object[] { logWriter });
                tests.Add(test);
            }
            return tests.ToArray();
        }
        private static void PrintResults(Dictionary<Test, TestResult> results, TestResult which)
        {
            var matching = results.Where(kv => kv.Value == which).ToArray();
            Console.WriteLine($"{which} : {matching.Length}");
            foreach (var r in matching)
            {
                Console.WriteLine($" + {r.Key.GetType().Name}");
            }
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
        private static void Die(int exitCode)
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(intercept: true);
            Environment.Exit(exitCode);
        }
    }
}
