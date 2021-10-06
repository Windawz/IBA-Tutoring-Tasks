using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CSStarterTest1.Tester
{
    internal static class AssemblyLoader
    {
        public static AssemblyLoadInfo Load(AssemblyName name)
        {
            Assembly? asm = null;
            try
            {
                asm = Assembly.Load(name);
            }
            catch (Exception ex) when (ex is FileNotFoundException or FileLoadException or BadImageFormatException) { }

            return new AssemblyLoadInfo(name, asm);
        }
    }
}
