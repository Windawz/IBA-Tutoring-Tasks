using System;
using System.Collections.Generic;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages
{
    internal class PerformTestsAndGetStatusStage : Stage<NamedTest, Nothing>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Performing tests",
            StageMessage.Results => "Failed tests",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IStageOutput<Nothing>[] Process(NamedTest[] input)
        {
            var results = input
                .Select(t => new Output(t, TestPerformer.Perform(t.Test)));
            return results.ToArray();
        }

        private class Output : IStageOutput<Nothing>
        {
            public Output(NamedTest test, TestResult result)
            {
                _testName = test.Name;
                _result = result;
            }

            private string _testName;
            private TestResult _result;

            public Nothing Data { get; } = Nothing.Instance;

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = _testName,
                Color = _result switch
                {
                    TestResult.Success => ConsoleColor.Green,
                    TestResult.Failure => ConsoleColor.Red,
                    _ => throw new InvalidOperationException($"{nameof(TestResult)} value out of range"),
                },
            };
        }
    }
}
