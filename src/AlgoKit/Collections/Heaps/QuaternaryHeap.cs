using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents an implicit heap-ordered complete quaternary tree.
    /// </summary>
    public class QuaternaryHeap<TKey, TValue> : ArrayHeap<TKey, TValue>
    {
        /// <summary>
        /// Creates an empty quaternary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public QuaternaryHeap(IComparer<TKey> comparer) : base(comparer, 4)
        {
        }

        /// <summary>
        /// Creates a quaternary heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public QuaternaryHeap(IComparer<TKey> comparer, IReadOnlyCollection<KeyValuePair<TKey, TValue>> items) : base(comparer, 4, items)
        {
        }
    }
}