using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CSStarterTest1.Tester
{
    public class Program
    {
        public static void Main()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Assembly asm = Assembly.GetExecutingAssembly();
            Console.WriteLine($"Executing assembly: \"{asm.GetName()}\"");

            ITest[] tests = asm
                .GetTypes()
                .Where(t => t.GetInterface(nameof(ITest)) is not null)
                .Select(t => 
                    (ITest)t
                    .GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>()) // If null then something's really wrong
                )
                .ToArray();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine($"{tests.Length} test(s) detected:");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            foreach (ITest t in tests)
            {
                Console.WriteLine($" + {t.GetType().Name}");
            }
            Console.ForegroundColor = defaultColor;

            Console.WriteLine();
            Console.WriteLine($"Performing...");
            Console.WriteLine();

            Dictionary<ITest, TestResult> results = new();

            void PrintTestsOf(TestResult tr)
            {
                var matching = results.Where(kv => kv.Value == tr).ToArray();
                Console.WriteLine($"{tr} : {matching.Length}");
                foreach (var r in matching)
                {
                    Console.WriteLine($" + {r.Key.GetType().Name}");
                }
            }

            foreach (ITest t in tests)
            {
                results[t] = t.Perform();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            PrintTestsOf(TestResult.Success);
            Console.ForegroundColor = ConsoleColor.Red;
            PrintTestsOf(TestResult.Failure);

            Console.ForegroundColor = defaultColor;

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}
