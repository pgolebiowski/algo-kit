using System.Collections.Generic;

namespace AlgoKit.Algorithms.Permutations
{
    /// <summary>
    /// Represents an algorithm for generating a random permutation
    /// of a finite collection (in-place shuffling).
    /// </summary>
    public interface IShuffleAlgorithm
    {
        /// <summary>
        /// Given a preinitialized collection, shuffles the elements of the collection in place,
        /// rather than producing its shuffled copy.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The preinitialized collection to be shuffled.</param>
        void Shuffle<T>(IList<T> collection);
    }
}