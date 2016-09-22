using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Algorithms.Permutations;
using AlgoKit.Extensions;
using NUnit.Framework;

namespace AlgoKit.Test.Algorithms.Permutations
{
    [TestFixture]
    public class PermutationLexicographicOrderingTests
    {
        public static IEnumerable<TestCaseData> GetTestCases()
            => Enumerable.Range(1, 6).Select(x => new TestCaseData(x));

        [TestCaseSource(nameof(GetTestCases))]
        public void ShouldProperlyFindNextPermutationInLexicographicOrder(int count)
        {
            // Arrange
            var permutations = GetPermutations(Enumerable.Range(1, count).ToArray(), count)
                .ToArray();

            for (var i = 0; i < permutations.Length - 1; ++i)
            {
                var input = permutations[i].ToArray();
                var expected = permutations[i + 1];

                // Act
                var wasFoundNext = PermutationLexicographicOrdering.NextPermutation(input);

                // Assert
                Assert.True(wasFoundNext);
                Assert.AreEqual(Stringify(expected), Stringify(input));
            }
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void ShouldNotModifyPermutationIfGivenOneIsLastAlready(int count)
        {
            // Arrange
            var original = Enumerable.Range(1, count).OrderByDescending(x => x).ToArray();
            var copy = new int[count];
            Array.Copy(original, copy, count);

            // Act
            var wasFoundNext = PermutationLexicographicOrdering.NextPermutation(copy);

            // Assert
            Assert.False(wasFoundNext);
            Assert.AreEqual(Stringify(original), Stringify(copy));
        }

        [TestCaseSource(nameof(GetTestCases))]
        public void ShouldEnumeratePermutationsInLexicographicOrder(int count)
        {
            // Arrange
            var expected = GetPermutations(Enumerable.Range(1, count).ToArray(), count)
                .Select(Stringify)
                .ToArray();
            
            var input = Enumerable.Range(1, count).ToArray();

            // Act
            var actual = PermutationLexicographicOrdering.EnumeratePermutations(input)
                .Select(Stringify)
                .ToArray();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }

        private static IEnumerable<IEnumerable<T>> GetPermutations<T>(IList<T> list, int length)
        {
            // Base case - break the list down into single pieces
            if (length == 1)
                return list.Select(x => x.Yield());

            return GetPermutations(list, length - 1)
                .SelectMany(list.Except, (a, b) => a.Concat(b.Yield()));
        }

        private static string Stringify(IEnumerable<int> sequence) => string.Join("", sequence);
    }
}
