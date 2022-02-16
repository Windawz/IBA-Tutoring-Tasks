﻿using System;
using System.IO;

namespace CSStarterTest1.TestUtils
{
    public abstract class Test
    {
        protected Test(TextWriter writer)
        {
            Logger = new(writer, Name);
        }

        public string Name => GetType().Name;
        protected TestLogger Logger { get; }

        public abstract TestResult Perform();
    }
}
