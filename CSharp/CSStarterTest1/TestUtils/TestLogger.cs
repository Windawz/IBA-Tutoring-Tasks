using System;
using System.IO;

namespace CSStarterTest1.TestUtils
{
    public sealed class TestLogger : IDisposable
    {
        internal TestLogger(TextWriter writer, string tag)
        {
            _writer = writer;
            _tagPrefix = BuildPrefix(tag);
        }

        private readonly TextWriter _writer;
        private readonly string _tagPrefix;
        private bool _cleanLine = true;

        public TestLogger Write(char c)
        {
            if (_cleanLine)
            {
                _writer.Write(_tagPrefix);
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
        public void Dispose()
        {
            _writer.Dispose();
        }

        private static string BuildPrefix(string testName) =>
            $"[ {testName} ]: ";
    }
}
