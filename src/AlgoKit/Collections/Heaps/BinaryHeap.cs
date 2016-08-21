using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents an implicit heap-ordered complete binary tree.
    /// </summary>
    public class BinaryHeap<TKey, TValue> : ArrayHeap<TKey, TValue>
    {
        /// <summary>
        /// Creates an empty binary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public BinaryHeap(IComparer<TKey> comparer) : base(comparer, 2)
        {
        }

        /// <summary>
        /// Creates a binary heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public BinaryHeap(IComparer<TKey> comparer, IReadOnlyCollection<KeyValuePair<TKey, TValue>> items) : base(comparer, 2, items)
        {
        }
    }
}
