using System;

namespace CSStarterTest1.Tester
{
    internal class TestTest : ITest
    {
        public TestResult Perform()
        {
            return (TestResult)new Random(DateTime.Now.Second).Next(0, 2);
        }
    }
}
