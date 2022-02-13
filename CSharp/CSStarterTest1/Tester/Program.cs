using System;

namespace CSStarterTest1.Tester
{
    public sealed class Program
    {
        public static void Main()
        {
            using var app = new Application();
            app.Run();
            Environment.ExitCode = app.ExitCode;
        }
    }
}
