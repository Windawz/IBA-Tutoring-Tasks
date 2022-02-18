using System;
using System.IO;
using System.Linq;
using System.Reflection;

using CSStarterTest1.TestUtils;

namespace CSStarterTest1.Tester
{
    internal class TestType
    {
        private static readonly Type[] _testCtorParameterTypes =
            typeof(Test)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single()
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

        public TestType(Type type)
        {
            ConstructorInfo? info = type.GetConstructor(_testCtorParameterTypes);
            if (info is null)
            {
                throw new ArgumentException($"Type has no constructor with matching parameter types", nameof(type));
            }

            if (!IsValidCtor(info))
            {
                throw new ArgumentException($"Matched constructor is inaccessible or invalid", nameof(type));
            }

            _ctor = info;
            Type = type;
        }

        private readonly ConstructorInfo _ctor;

        public Type Type { get; }

        public Test Instantiate(TextWriter logger)
        {
            var parameters = new object[]{ logger };
            return (Test)_ctor.Invoke(parameters);
        }
        public static bool IsValid(Type type)
        {
            ConstructorInfo? info = type.GetConstructor(_testCtorParameterTypes);
            return info is not null && IsValidCtor(info);
        }

        private static bool IsValidCtor(ConstructorInfo info)
        {
            return info.IsPublic && !info.IsStatic;
        }
    }
}
