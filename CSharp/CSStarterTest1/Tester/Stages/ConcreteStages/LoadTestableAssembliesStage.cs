using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSStarterTest1.Tester.Stages.ConcreteStages
{
    internal class LoadTestableAssembliesStage : Stage<AssemblyName, Assembly>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Loading",
            StageMessage.Results => "Loaded assemblies",
            _ => base.GetMessage(messageKind),
        };
        public override IStageOutput<Assembly>[] Process(AssemblyName[] input)
        {
            AssemblyLoadInfo[] infos = input.Select(name => AssemblyLoader.Load(name)).ToArray();
            return infos
                .Select(info => new Output(info))
                .ToArray();
        }

        private class Output : IStageOutput<Assembly>
        {
            public Output(AssemblyLoadInfo info)
            {
                _assemblyName = info.Name.Name!;
                Data = info.LoadedAssembly;
            }

            private string _assemblyName;

            public Assembly? Data { get; }

            public StageOutputDisplayInfo GetDisplayInfo() => new()
            {
                Text = _assemblyName,
                Color = Data is null ? ConsoleColor.Red : ConsoleColor.Green,
            };
        }
    }
}
