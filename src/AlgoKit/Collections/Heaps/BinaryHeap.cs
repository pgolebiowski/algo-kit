using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap-ordered complete binary tree.
    /// </summary>
    public class BinaryHeap<T> : DAryHeap<T>
    {
        /// <summary>
        /// Creates an empty binary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public BinaryHeap(IComparer<T> comparer) : base(2, comparer)
        {
        }

        /// <summary>
        /// Creates a binary heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public BinaryHeap(List<T> items, IComparer<T> comparer) : base(2, items, comparer)
        {
        }
    }
}
