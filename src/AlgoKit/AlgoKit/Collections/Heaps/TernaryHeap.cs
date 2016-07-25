using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap-ordered complete ternary tree.
    /// </summary>
    public class TernaryHeap<T> : DAryHeap<T>
    {
        /// <summary>
        /// Creates an empty ternary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public TernaryHeap(IComparer<T> comparer) : base(3, comparer)
        {
        }

        /// <summary>
        /// Creates a ternary heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public TernaryHeap(List<T> items, IComparer<T> comparer) : base(3, items, comparer)
        {
        }
    }
}