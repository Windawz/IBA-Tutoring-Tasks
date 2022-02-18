using System;
using System.Collections.Generic;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class PerformTestsAndGetStatusStage : Stage<Test, Nothing>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Performing tests",
            StageMessage.Results => "Test results",
            _ => base.GetMessage(messageKind),
        };
        public override IStageOutput<Nothing>[] Process(Test[] input)
        {
            Output[] results;
            try
            {
                results = input.Select(t => new Output(t.Name, t.Perform())).ToArray();
            }
            finally
            {
                foreach (Test test in input)
                {
                    test.Dispose();
                }
            }
            return results.ToArray();
        }

        private class Output : IStageOutput<Nothing>
        {
            public Output(string testName, TestResult result)
            {
                _testName = testName;
                _result = result;
            }

            private string _testName;
            private TestResult _result;

            public Nothing Data { get; } = Nothing.Instance;

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = _testName,
                Color = _result.Status switch
                {
                    TestStatus.Success => ConsoleColor.Green,
                    TestStatus.Failure => ConsoleColor.Red,
                    _ => throw new InvalidOperationException($"{nameof(TestResult)} value out of range"),
                },
            };
        }
    }
}
