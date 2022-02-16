using System;

namespace CSStarterTest1.Tester
{
    internal class TestLogNameGenerator
    {
        public string GetLogName(string testName)
        {
            return $"Log_{testName}_{DateTime.Now:dd-MM-yy_HH'h'mm'm'ss's'}.log";
        }
    }
}
