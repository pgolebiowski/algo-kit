using System;

namespace AlgoKit.Test
{
    /// <summary>
    /// Just a small wrapper for <see cref="System.Random"/>, as it lacks the seed property.
    /// </summary>
    internal class RandomWithSeed : Random
    {
        public int Seed { get; }

        public RandomWithSeed() : this(new Random().Next())
        {
            
        }

        public RandomWithSeed(int seed) : base(seed)
        {
            this.Seed = seed;
        }
    }
}