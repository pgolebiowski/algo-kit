using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap where the parent key is less than or equal
    /// to the child keys.
    /// </summary>
    public class MinBinaryHeap<T> : BinaryHeap<T>, IMinHeap<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Creates an empty heap.
        /// </summary>
        public MinBinaryHeap()
        {
            this.Items = new List<T>();
        }

        /// <summary>
        /// Creates a heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        public MinBinaryHeap(List<T> items)
        {
            this.Items = items;
            this.Heapify();
        }

        /// <summary>
        /// Gets the minimum item of the heap.
        /// </summary>
        public T Minimum
        {
            get
            {
                if (this.IsEmpty)
                    throw new InvalidOperationException("The heap is empty.");

                return this.Items[0];
            }
        }

        /// <summary>
        /// Returns the minimum item after removing it from the heap.
        /// </summary>
        public T ExtractMin()
        {
            var result = this.Minimum;
            this.DeleteMin();
            return result;
        }

        /// <summary>
        /// Removes the minimum (the root node) from the heap. 
        /// </summary>
        public void DeleteMin()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            this.RemoveAt(0);
        }

        protected override bool WouldBeExtractedEarlier(T first, T second)
        {
            return first.CompareTo(second) < 0;
        }
    }
}
