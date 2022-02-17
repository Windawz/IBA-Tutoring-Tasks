using System;
using System.CodeDom.Compiler;
using System.IO;

namespace CSStarterTest1.Tester
{
    internal class IndentControl
    {
        public const int DefaultLevelStep = 4;

        public IndentControl(IndentedTextWriter writer)
        {
            _writer = writer;
        }

        private IndentedTextWriter _writer;

        public int Indent
        {
            get => _writer.Indent;
            set => _writer.Indent = value;
        }
        public int LevelStep { get; set; } = DefaultLevelStep;

        public void IncreaseLevel(int steps = 1) => Indent += LevelStep * steps;
        public void DecreaseLevel(int steps = 1) => Indent -= LevelStep * steps;
    }
}
