using System;
using System.Text;

namespace CSharpPilot2.IO
{
    sealed class ConsoleOutputTarget : IOutputTarget
    {
        public ConsoleOutputTarget(Encoding encoding) =>
            _encoding = encoding;
        
        private readonly Encoding _encoding;

        public void Put(IOutput output)
        {
            Output info = output.Info;
            string text = info.Text;

            ConsoleColor oldColor = Console.ForegroundColor;
            ConsoleColor color = info.Color is null ? oldColor : TranslateColor((OutputColor)info.Color);

            Encoding oldEncoding = Console.OutputEncoding;

            Console.ForegroundColor = color;
            Console.OutputEncoding = _encoding;

            Console.Write(text);
            if (info.NewLine)
            {
                Console.WriteLine();
            }

            Console.ForegroundColor = oldColor;
            Console.OutputEncoding = oldEncoding;
        }

        static ConsoleColor TranslateColor(OutputColor color) =>
            color switch
            {
                OutputColor.Gray => ConsoleColor.Gray,
                OutputColor.Red => ConsoleColor.Red,
                OutputColor.Green => ConsoleColor.Green,
                OutputColor.Blue => ConsoleColor.Blue,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(color), $"Failed to translate unknown {nameof(OutputColor)} `{color}` to {nameof(ConsoleColor)}"),
            };
    }
}
