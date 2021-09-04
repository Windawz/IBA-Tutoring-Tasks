using System;

namespace CSharpPilot2
{
    sealed record ParsedArg(string Value)
    {
        public bool TryAsInteger(out int output) =>
            Int32.TryParse(Value, out output);
        public bool TryAsDouble(out double output) =>
            Double.TryParse(Value, out output);

        public int AsIntegerOrDefault() =>
            TryAsInteger(out int output) ? output : default;
        public double AsDoubleOrDefault() =>
            TryAsDouble(out double output) ? output : default;
    }
}
