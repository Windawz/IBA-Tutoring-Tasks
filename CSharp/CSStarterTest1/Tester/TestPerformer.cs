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
            catch (Exception ex)
            {
                result = new TestResult(TestStatus.Failure, ex);
            }
            return result;
        }
    }
}
