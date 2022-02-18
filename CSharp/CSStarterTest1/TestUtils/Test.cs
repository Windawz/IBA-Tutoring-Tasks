using System;
using System.IO;

namespace CSStarterTest1.TestUtils
{
    public abstract class Test : IDisposable
    {
        protected Test(TextWriter writer)
        {
            Logger = new(writer, Name);
        }

        public string Name => GetType().Name;
        protected TestLogger Logger { get; }

        public TestResult Perform()
        {
            TestResult result;
            try
            {
                result = PerformImpl();
            }
            catch (Exception ex)
            {
                result = new TestResult(TestStatus.Failure, "Unhandled exception occured in test", ex);
            }

            if (result.Status == TestStatus.Failure)
            {
                LogTestFailure(result);
            }

            return result;
        }
        public void Dispose() => Logger.Dispose();

        protected abstract TestResult PerformImpl();

        private void LogTestFailure(TestResult result)
        {
            Logger.WriteLine("Test failed");

            bool hasException = result.Exception is not null;
            bool sameMessage = hasException && ReferenceEquals(result.Message, result.Exception!.Message);

            if (!String.IsNullOrWhiteSpace(result.Message) && !sameMessage)
            {
                Logger.WriteLine("Message:")
                    .WriteLine($"{result.Message}");
            }
            if (hasException)
            {
                Logger.WriteLine("Exception:")
                    .WriteLine($"{result.Exception}");
            }
        }
    }
}
