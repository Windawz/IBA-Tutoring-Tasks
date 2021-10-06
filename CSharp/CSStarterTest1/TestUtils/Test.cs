using System;
using System.IO;

namespace CSStarterTest1.TestUtils
{
    public abstract class Test
    {
        protected Test(TextWriter writer)
        {
            Logger = new(writer, GetType().Name);
        }

        protected TestLogger Logger { get; }

        public abstract TestResult Perform();
    }
}
