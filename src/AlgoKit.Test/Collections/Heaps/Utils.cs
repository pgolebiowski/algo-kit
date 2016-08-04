using System.Collections.Generic;
using Ploeh.AutoFixture;

namespace AlgoKit.Test.Collections.Heaps
{
    public static class Utils
    {
        public static List<int> GenerateList(int size, int minimum = int.MinValue, int maximum = int.MaxValue)
        {
            var fixture = new Fixture { RepeatCount = size };
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(minimum, maximum));
            return fixture.Create<List<int>>();
        }
    }
}