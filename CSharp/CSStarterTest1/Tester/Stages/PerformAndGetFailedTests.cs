using System;
using System.Collections.Generic;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester.Stages
{
    internal class PerformAndGetFailedTests : Stage<Test, Test>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Performing tests",
            StageMessage.Results => "Failed tests",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IEnumerable<Test> Process(IEnumerable<Test> input)
        {
            return TestPerformer.Perform(input)
                .Where(kv => kv.Value == TestResult.Failure)
                .Select(kv => kv.Key)
                .ToArray()
                ;
        }
    }
}
