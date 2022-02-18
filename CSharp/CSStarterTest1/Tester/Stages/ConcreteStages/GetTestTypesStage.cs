using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class GetTestTypesStage : Stage<Assembly, TestType>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Getting test types",
            StageMessage.Results => "Found test types",
            _ => base.GetMessage(messageKind),
        };
        public override IStageOutput<TestType>[] Process(Assembly[] input)
        {
            return input
                .SelectMany(a => TestFinder.LoadTestTypes(a))
                .Where(t => TestType.IsValid(t))
                .Select(t => new TestType(t))
                .Select(t => new Output(t))
                .ToArray();
        }

        private class Output : IStageOutput<TestType>
        {
            public Output(TestType testType) => Data = testType;

            public TestType? Data { get; }

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = Data!.Type.Name,
                Color = ConsoleColor.Green,
            };
        }
    }
}
