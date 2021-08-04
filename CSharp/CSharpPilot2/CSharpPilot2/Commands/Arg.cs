using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPilot2
{
    internal class Arg
    {
        public Arg(string value) =>
            Value = value;

        public string Value { get; init; }

        public bool TryAsInteger(out int output) =>
            int.TryParse(Value, out output);
        public bool TryAsDouble(out double output) =>
            double.TryParse(Value, out output);

        public int AsIntegerOrDefault() =>
            TryAsInteger(out int output) ? output : default;
        public double AsDoubleOrDefault() =>
            TryAsDouble(out double output) ? output : default;
    }
}
