using System;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal static class TestResultExtensions
    {
        public static ConsoleColor GetConsoleColor(this TestResult testResult) => testResult switch {
            TestResult.Success => ConsoleColor.Green,
            TestResult.Failure => ConsoleColor.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(testResult)),
        };
    }
}
