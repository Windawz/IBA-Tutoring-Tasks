using System;
using System.Reflection;

namespace CSStarterTest1.Tester
{
    internal sealed record AssemblyLoadInfo(AssemblyName Name, Assembly? LoadedAssembly);
}
