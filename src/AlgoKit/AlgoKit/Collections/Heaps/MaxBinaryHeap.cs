using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap where the parent key is greater than or equal
    /// to the child keys.
    /// </summary>
    public class MaxBinaryHeap<T> : BinaryHeap<T>, IMaxHeap<T>
        where T : IComparable<T>
    {
        /// <summary>
        /// Creates an empty heap.
        /// </summary>
        public MaxBinaryHeap()
        {
            this.Items = new List<T>();
        }

        /// <summary>
        /// Creates a heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="items">The list of items to build a heap from.</param>
        public MaxBinaryHeap(List<T> items)
        {
            this.Items = items;
            this.Heapify();
        }

        /// <summary>
        /// Gets the maximum item of the heap.
        /// </summary>
        public T Maximum
        {
            get
            {
                if (this.IsEmpty)
                    throw new InvalidOperationException("The heap is empty.");

                return this.Items[0];
            }
        }

        /// <summary>
        /// Returns the maximum item after removing it from the heap.
        /// </summary>
        public T ExtractMax()
        {
            var result = this.Maximum;
            this.DeleteMax();
            return result;
        }

        /// <summary>
        /// Removes the maximum (the root node) from the heap. 
        /// </summary>
        public void DeleteMax()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            this.RemoveAt(0);
        }

        protected override bool WouldBeExtractedEarlier(T first, T second)
        {
            return first.CompareTo(second) > 0;
        }
    }
}