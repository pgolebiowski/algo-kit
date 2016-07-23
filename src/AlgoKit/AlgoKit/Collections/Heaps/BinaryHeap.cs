using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap data structure that takes the form of a binary tree.
    /// </summary>
    /// <remarks>
    /// • Shape property: a binary heap is a complete binary tree; that is, all
    /// levels of the tree, except possibly the last one (deepest) are fully
    /// filled, and, if the last level of the tree is not complete, the nodes
    /// of that level are filled from left to right.
    /// 
    /// • Heap property: the key stored in each node is either greater than
    /// or equal to or less than or equal to the keys in the node's children,
    /// according to some total order.
    /// </remarks>
    public abstract class BinaryHeap<T> : IEnumerable<T>
        where T : IComparable<T>
    {
        protected List<T> Items;

        /// <summary>
        /// Gets the number of elements in the heap.
        /// </summary>
        public int Count => this.Items.Count;

        /// <summary>
        /// Returns true if the heap is empty, false otherwise.
        /// </summary>
        public bool IsEmpty => this.Items.Count == 0;

        /// <summary>
        /// Determines whether an element is in the heap.
        /// </summary>
        public bool Contains(T item) => this.Items.Contains(item);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index
        /// of the first occurrence within the entire heap.
        /// </summary>
        public int IndexOf(T item) => this.Items.IndexOf(item);

        /// <summary>
        /// Removes all elements from the heap.
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
        }

        /// <summary>
        /// Determines if the first item would be extracted earlier. This means that
        /// in general it would also be closer to the root. The implementation
        /// of this method is different for min and max heaps.
        /// </summary>
        protected abstract bool WouldBeExtractedEarlier(T first, T second);

        /// <summary>
        /// Adds an item to the heap.
        /// </summary>
        public void Insert(T item)
        {
            // Add the item to the bottom level of the heap
            var index = this.Items.Count;
            this.Items.Add(item);

            if (index == 0)
                return;

            this.SiftUp(index);
        }

        /// <summary>
        /// Removes an arbitrary node.
        /// </summary>
        /// <param name="index">The index of the node to be removed.</param>
        public void RemoveAt(int index)
        {
            // Replace the root of the heap by the last element on the last level.
            var lastIndex = this.Items.Count - 1;
            this.Items[index] = this.Items[lastIndex];
            this.Items.RemoveAt(lastIndex);

            // The heap property needs to be restored (in general) if there are any
            // remaining nodes.
            if (lastIndex > 0)
                this.SiftDown(index);
        }

        /// <summary>
        /// Updates the value of the node with the specified index and restores the heap
        /// property. The new value can be greater, lesser, or equal to the current one.
        /// </summary>
        /// <param name="index">The index of the node to be modified.</param>
        /// <param name="item">The new value of the node.</param>
        public void Update(int index, T item)
        {
            var oldItem = this.Items[index];
            this.Items[index] = item;

            if (this.WouldBeExtractedEarlier(item, oldItem))
            {
                this.SiftUp(index);
            }
            else if (this.WouldBeExtractedEarlier(oldItem, item))
            {
                this.SiftDown(index);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the heap.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => this.Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Uses sifting down in a bottom-up manner to convert an unordered list into a heap.
        /// </summary>
        protected void Heapify()
        {
            // Consider leaves of the tree. These are 1-element heaps, so there
            // is no need to correct them. The heap property needs to be restored
            // only for higher nodes, starting from the first node that has children.
            // It is the parent of the very last element in the array.

            var lastIndex = this.Items.Count - 1;

            for (var index = GetParentIndex(lastIndex); index >= 0; --index)
                this.SiftDown(index);
        }

        /// <summary>
        /// Moves a node up in the tree, as long as needed. Used to restore heap condition
        /// after insertion.
        /// </summary>
        /// <param name="index">The index of the node to be sifted up.</param>
        private void SiftUp(int index)
        {
            var toSift = this.Items[index];

            // Instead of swapping items all th way to the root, we will perform
            // a similar optimization as in the insertion sort.

            while (index > 0)
            {
                var parentIndex = GetParentIndex(index);
                var parent = this.Items[parentIndex];

                if (this.WouldBeExtractedEarlier(toSift, parent))
                {
                    this.Items[index] = parent;
                    index = parentIndex;
                }
                else
                    break;
            }

            this.Items[index] = toSift;
        }

        /// <summary>
        /// Move a node down in the tree. Used to restore heap condition
        /// after deletion or replacement.
        /// </summary>
        /// <param name="index">The index of the node to be sifted down.</param>
        private void SiftDown(int index)
        {
            // The node to sift down will not actually be swapped every time.
            // Rather, values on the 'sifting path' will move up, leaving a free spot
            // for this value to drop in. Similar optimization as in the insertion sort.
            var toSiftDown = this.Items[index];

            // Top element, would be extracted first.
            var topIndex = index;
            var top = toSiftDown;

            while (true)
            {
                // Check if the current node would really be extracted first, or maybe
                // one of its children would be extracted earlier.
                var leftChildIndex = GetLeftChildIndex(index);
                var rightChildIndex = GetRightChildIndex(index);

                if (leftChildIndex < this.Items.Count)
                {
                    var leftChild = this.Items[leftChildIndex];
                    if (this.WouldBeExtractedEarlier(leftChild, top))
                    {
                        topIndex = leftChildIndex;
                        top = leftChild;
                    }
                }

                if (rightChildIndex < this.Items.Count)
                {
                    var rightChild = this.Items[rightChildIndex];
                    if (this.WouldBeExtractedEarlier(rightChild, top))
                    {
                        topIndex = rightChildIndex;
                        top = rightChild;
                    }
                }

                // In case the current node would really be extracted first, there is
                // nothing more to do - a free spot for it was found.
                if (topIndex == index)
                    break;

                // Move the top value up by one node and now investigate the
                // node that was considered to be top (recursive).
                this.Items[index] = top;
                index = topIndex;
                top = toSiftDown;
            }

            this.Items[index] = toSiftDown;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetParentIndex(int index) => (index - 1)/2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetLeftChildIndex(int index) => 2*index + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRightChildIndex(int index) => 2*index + 2;
    }
}