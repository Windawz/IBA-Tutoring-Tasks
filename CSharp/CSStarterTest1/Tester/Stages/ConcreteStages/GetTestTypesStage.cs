using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class GetTestTypesStage : Stage<Assembly, Type>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Getting test types",
            StageMessage.Results => "Found test types",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IStageOutput<Type>[] Process(Assembly[] input)
        {
            return input
                .SelectMany(a => TestFinder.LoadTestTypes(a))
                .Select(t => new Output(t))
                .ToArray();
        }

        private class Output : IStageOutput<Type>
        {
            public Output(Type type) => Data = type;

            public Type? Data { get; }

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = Data!.Name,
                Color = ConsoleColor.Green,
            };
        }
    }
}
