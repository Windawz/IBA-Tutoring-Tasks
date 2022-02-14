using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSStarterTest1.Tester.Stages
{
    internal class LoadTestableAssembliesStage : Stage<AssemblyName, Assembly>
    {
        public override string GetMessage(StageMessage messageKind) => messageKind switch
        {
            StageMessage.Starting => "Loading",
            StageMessage.Results => "Loaded assemblies",
            _ => throw new ArgumentOutOfRangeException(nameof(messageKind)),
        };
        public override IEnumerable<Assembly> Process(IEnumerable<AssemblyName> input)
        {
            AssemblyLoadInfo[] infos = input.Select(name => AssemblyLoader.Load(name)).ToArray();
            return infos
                .Where(i => i.LoadedAssembly is not null)
                .Select(i => i.LoadedAssembly!)
                .ToArray();
        }
    }
}
