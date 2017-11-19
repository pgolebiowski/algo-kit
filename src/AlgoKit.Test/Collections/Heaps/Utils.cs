using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoKit.Test.Collections.Heaps
{
    public static class Utils
    {
        public static List<int> GenerateList(int size, int minimum = int.MinValue, int maximum = int.MaxValue)
        {
            var random = new Random();
            return Enumerable.Range(1, size)
                .Select(x => random.Next(minimum, maximum))
                .ToList();
        }
    }
}