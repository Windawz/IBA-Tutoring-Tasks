using System;
using System.IO;
using System.Text;

namespace CSStarterTest1.Tester
{
    internal abstract class Test
    {
        protected Test(TextWriter writer)
        {
            Logger = new(writer, GetType().Name);
        }

        protected TestLogger Logger { get; } 

        public abstract TestResult Perform();
    }
}
