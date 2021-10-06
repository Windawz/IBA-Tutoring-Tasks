using System;
using System.IO;
using System.Text;

namespace CSStarterTest1.Tester
{
    internal static class WriterProvider
    {
        public static TextWriter TestWriter { get; } = TryGetLogWriter("tests.log") ?? TextWriter.Null;
        public static TextWriter MessageWriter { get; } = Console.Out;

        private static TextWriter? TryGetLogWriter(string fileName)
        {
            Stream? stream = null;

            try
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
            }
            catch (SystemException)
            {
                stream?.Dispose();
                MessageWriter.WriteLine("Failed to create test log file!");
                return null;
            }
            
            return new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: false);
        }
    }
}
