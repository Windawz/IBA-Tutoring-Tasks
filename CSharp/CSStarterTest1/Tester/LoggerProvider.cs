using System;
using System.IO;
using System.Text;

namespace CSStarterTest1.Tester
{
    internal class LoggerProvider
    {
        public TextWriter GetLogger(string logFileName)
        {
            FileStream stream = File.OpenWrite(logFileName);
            return new StreamWriter(stream, encoding: Encoding.UTF8, leaveOpen: false);
        }
    }
}
