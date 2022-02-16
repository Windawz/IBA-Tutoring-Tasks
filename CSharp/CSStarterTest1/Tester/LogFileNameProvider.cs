using System;

namespace CSStarterTest1.Tester
{
    internal class LogFileNameProvider
    {
        public string GetName(string testName)
        {
            return $"Log_{testName}_{DateTime.Now:dd-MM-yy_HH'h'mm'm'ss's'}.log";
        }
    }
}
