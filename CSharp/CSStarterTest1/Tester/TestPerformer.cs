using System;
using System.Collections.Generic;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal static class TestPerformer
    {
        public static TestResult Perform(Test test)
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
            return result;
        }

        public static Dictionary<Test, TestResult> Perform(IEnumerable<Test> tests)
        {
            var dict = new Dictionary<Test, TestResult>();
            foreach (Test test in tests)
            {
                dict[test] = Perform(test);
            }
            return dict;
        }
    }
}
