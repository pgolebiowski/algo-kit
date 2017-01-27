using System;

namespace AlgoKit.Extensions
{
    public static class StringExtensions
    {
        public static string ToReversed(this string text)
        {
            var array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}