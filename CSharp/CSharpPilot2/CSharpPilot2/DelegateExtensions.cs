using System;

namespace CSharpPilot2
{
    static class DelegateExtensions
    {
        public static T Combine<T>(this T left, T right) where T : Delegate =>
            (T)Delegate.Combine(left, right);
    }
}
