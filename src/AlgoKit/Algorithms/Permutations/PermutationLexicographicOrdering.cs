using System;
using System.Collections.Generic;

namespace AlgoKit.Algorithms.Permutations
{
    /// <summary>
    /// Provides methods for generating permutations in lexicographic order.
    /// </summary>
    public static class PermutationLexicographicOrdering
    {
        /// <summary>
        /// Rearranges the given permutation to the next one in lexicographic order
        /// and returns true. In case the permutation is the last one (all items are in decreasing order),
        /// it does nothing and returns false.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="permutation">An array containing a sequence of elements.</param>
        public static bool NextPermutation<T>(T[] permutation) where T : IComparable<T>
        {
            if (permutation == null)
                throw new ArgumentNullException(nameof(permutation));

            var n = permutation.Length;
            var k = -1;

            // Find the largest index k such that a[k] < a[k + 1]
            for (var i = 1; i < n; ++i)
                if (permutation[i - 1].CompareTo(permutation[i]) < 0)
                    k = i - 1;

            // If no such index exists, the permutation is the last one
            if (k == -1)
                return false;

            // Find the largest index l greater than k such that a[k] < a[l]
            var l = k + 1;
            for (var i = l; i < n; i++)
                if (permutation[k].CompareTo(permutation[i]) < 0)
                    l = i;

            // Swap the value of a[k] with that of a[l]
            var tmp = permutation[k];
            permutation[k] = permutation[l];
            permutation[l] = tmp;

            // Reverse the sequence from a[k + 1] up to and including the final element a[n]
            Array.Reverse(permutation, k + 1, n - (k + 1));

            // The permutation is not the last one
            return true;
        }

        /// <summary>
        /// Enumerates permutations of the given set in lexicographic order, starting at
        /// the given permutation. Yielded arrays are the same instance - only rearranged.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="permutation">An array containing a sequence of elements.</param>
        public static IEnumerable<T[]> EnumeratePermutations<T>(T[] permutation) where T : IComparable<T>
        {
            if (permutation == null)
                throw new ArgumentNullException(nameof(permutation));

            do
            {
                yield return permutation;
            } while (NextPermutation(permutation));
        }
    }
}
