
using System;

namespace CSStarterTest1.TestUtils
{
    public class TestResult
    {
        public TestResult(TestStatus status, string message, Exception? exception)
        {
            Status = status;
            Message = message;
            Exception = exception;
        }
        public TestResult(TestStatus status, Exception exception) : this(status, exception.Message, exception) { }
        public TestResult(TestStatus status, string message) : this(status, message, null) { }
        public TestResult(TestStatus status) : this(status, "") { }

        public TestStatus Status { get; init; }
        public string Message { get; init; }
        public Exception? Exception { get; init; } = null;
    }
}
