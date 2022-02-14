using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSStarterTest1.Tester.Stages
{
    internal class GetTestTypesStage : Stage<Assembly, Type>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Getting test types",
            StageMessage.Results => "Found test types",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IEnumerable<Type> Process(IEnumerable<Assembly> input)
        {
            return input
                .SelectMany(a => TestFinder.LoadTestTypes(a))
                .ToArray();
        }
    }
}
