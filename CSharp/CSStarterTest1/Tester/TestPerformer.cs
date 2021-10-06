using System;
using System.Collections.Generic;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal static class TestPerformer
    {
        public static Dictionary<Test, TestResult> Perform(IEnumerable<Test> tests)
        {
            Dictionary<Test, TestResult> dict = new();
            foreach (Test test in tests)
            {
                TestResult result;
                try
                {
                    result = test.Perform();
                }
                catch (Exception)
                {
                    result = TestResult.Failure;
                }

                dict[test] = result;
            }
            return dict;
        }
    }
}
