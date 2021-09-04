using System;

using static System.Console;

namespace CSharpPilot2.IO
{
    sealed class ConsoleOutputTarget : IOutputTarget
    {
        public void Put(IOutput output)
        {
            OutputInfo info = output.Info;
            string text = info.Text;
            ConsoleColor color = info.Color is null ? _oldColor : TranslateColor((OutputColor)info.Color);

            SetColor(color);

            Write(text);
            if (info.NewLine)
            {
                WriteLine();
            }

            RestoreColor();
        }

        static ConsoleColor _oldColor = ForegroundColor;

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
        void SetColor(ConsoleColor color)
        {
            _oldColor = ForegroundColor;
            ForegroundColor = color;
        }
        void RestoreColor() => ForegroundColor = _oldColor;
    }
}
