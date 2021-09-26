using System;
using System.IO;
using System.Text;

namespace CSStarterTest1.Tester
{
    internal sealed class TestLogger 
    {
        public TestLogger(TextWriter writer, string testName)
        {
            _writer = writer;
            _prefix = GetPrefix(testName);
        }

        private readonly TextWriter _writer;
        private readonly string _prefix;
        private bool _cleanLine = true;

        public TestLogger Write(char c)
        {
            if (_cleanLine)
            {
                _writer.Write(_prefix);
                _cleanLine = false;
            }
            _writer.Write(c);

            return this;
        }
        public TestLogger Write(string s)
        {
            foreach (char c in s)
            {
                Write(c);
            }
            if (s.EndsWith(_writer.NewLine))
            {
                _cleanLine = true;
            }

            return this;
        }
        public TestLogger WriteLine(string s)
        {
            Write(s + _writer.NewLine);
            return this;
        }

        private static string GetPrefix(string testName) =>
            $"[ {testName} ]: ";
    }
}
