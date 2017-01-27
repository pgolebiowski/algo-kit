using System;
using System.Collections.Generic;
using AlgoKit.Extensions;

namespace AlgoKit.Algorithms.Permutations
{
    /// <summary>
    /// Linear algorithm producing an unbiased permutation. The shuffle is in place.
    /// Designing the algorithm involved Fisher, Yates, Durstenfeld, and Knuth.
    /// </summary>
    public class FisherYatesShuffle : IShuffleAlgorithm
    {
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="FisherYatesShuffle"/>,
        /// using a time-dependent default seed value for the pseudo-random number generator.
        /// </summary>
        public FisherYatesShuffle() : this(new Random())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FisherYatesShuffle"/>,
        /// using the specified pseudo-random number generator. 
        /// </summary>
        /// <param name="random">The pseudo-random number generator to use during shuffling.</param>
        public FisherYatesShuffle(Random random)
        {
            this.random = random;
        }

        /// <inheritdoc cref="IShuffleAlgorithm.Shuffle{T}"/>
        public void Shuffle<T>(IList<T> collection)
        {
            var n = collection.Count;

            for (var i = 0; i <= n - 2; ++i)
            {
                // random integer such that i ≤ j < n
                var j = this.random.Next(i, n);
                collection.Swap(i, j);
            }
        }
    }
}