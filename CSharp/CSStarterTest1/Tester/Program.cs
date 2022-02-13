using System;

namespace CSStarterTest1.Tester
{
    public sealed class Program
    {
        public static void Main()
        {
            string[] testedAssemblies =
            {
                "DatabaseInterface",
                "DataOps",
            };
            using var app = new Application(testedAssemblies);
            app.Run();
            Environment.ExitCode = app.ExitCode;
        }
    }
}
