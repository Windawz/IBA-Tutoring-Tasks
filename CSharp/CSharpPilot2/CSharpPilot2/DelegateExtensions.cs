using System;

namespace CSharpPilot2
{
    internal static class DelegateExtensions
    {
        public static T Combine<T>(this T left, T right) where T : Delegate =>
            (T)Delegate.Combine(left, right);
    }
}
