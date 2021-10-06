using System;
using System.IO;
using System.Reflection;
using System.Linq;

using CSStarterTest1.TestUtils;
using System.Collections.Generic;

namespace CSStarterTest1.Tester
{
    internal static class TestInstantiator
    {
        private static readonly Type[] _ctorParamTypes =
            typeof(Test)
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single()
                .GetParameters()
                .Select(p => p.ParameterType)
                .ToArray();

        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="testType"/> has no public constructor taking the same parameters as <see cref="Test(TextWriter)"/>.
        /// </exception>
        /// <exception cref="TargetInvocationException">
        /// See <see cref="ConstructorInfo.Invoke(object?[]?)"/>.
        /// </exception>
        /// <exception cref="MethodAccessException">
        /// See <see cref="ConstructorInfo.Invoke(object?[]?)"/>. This one cannot be caused by a private or protected constructor.
        /// </exception>
        public static Test Instantiate(Type testType, TextWriter logWriter)
        {
            ConstructorInfo? ctor = testType.GetConstructor(_ctorParamTypes);
            if (ctor is null || !ctor.IsPublic || ctor.IsStatic)
            {
                throw new ArgumentException($"Test \"{testType.Name}\" has no fitting constructor", nameof(testType));
            }

            return (Test)ctor.Invoke(new object[] { logWriter });
        }
    }
}
