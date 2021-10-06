using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    /// <summary>
    /// Looks for test types in the assemblies referenced by the calling assembly.
    /// </summary>
    internal static class TestFinder
    {
        public static Type[] LoadTestTypes(Assembly assembly) =>
            assembly.GetTypes().Where(t => IsTestType(t)).ToArray();
        private static bool IsTestType(Type type) =>
            type.IsClass &&
            type.BaseType == typeof(Test) &&
            type.IsPublic &&
            !type.IsAbstract;
    }
}
