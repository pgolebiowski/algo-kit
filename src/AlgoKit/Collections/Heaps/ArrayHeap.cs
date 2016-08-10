using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents an implicit heap-ordered complete d-ary tree, stored as an array.
    /// </summary>
    public class ArrayHeap<T> : BaseHeap<T, ArrayHeapNode<T>, ArrayHeap<T>>
    {
        private readonly List<ArrayHeapNode<T>> nodes;

        /// <summary>
        /// Creates an empty quaternary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        public ArrayHeap(IComparer<T> comparer) : this(comparer, 4)
        {
        }

        /// <summary>
        /// Creates an empty d-ary heap.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        /// <param name="arity">The arity of the heap.</param>
        public ArrayHeap(IComparer<T> comparer, int arity) : this(comparer, arity, new List<T>())
        {
        }

        /// <summary>
        /// Creates a d-ary heap out of given list of elements in linear time.
        /// </summary>
        /// <param name="comparer">The comparer used to determine whether one object should be extracted from the heap earlier than the other one.</param>
        /// <param name="arity">The arity of the heap.</param>
        /// <param name="items">The list of items to build a heap from.</param>
        public ArrayHeap(IComparer<T> comparer, int arity, IReadOnlyCollection<T> items)
        {
            if (arity < 1)
                throw new ArgumentOutOfRangeException($"Expected arity to be at least 1, but was {arity}.");

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            this.Arity = arity;
            this.nodes = items.Select((x, i) => new ArrayHeapNode<T>(x, i)).ToList();
            this.Comparer = comparer;

            if (this.Count > 1)
                this.Heapify();
        }

        /// <summary>
        /// Gets the arity of the heap.
        /// </summary>
        public int Arity { get; }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public override int Count => this.nodes.Count;

        /// <summary>
        /// Gets the top element of the heap.
        /// </summary>
        public override T Top()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            return this.nodes[0].Value;
        }

        /// <summary>
        /// Returns the top element after removing it from the heap.
        /// </summary>
        public override T Pop()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            var top = this.nodes[0];
            this.Remove(top);

            return top.Value;
        }

        /// <summary>
        /// Adds an object to the heap.
        /// </summary>
        public override ArrayHeapNode<T> Add(T value)
        {
            // Add node at the end
            var node = new ArrayHeapNode<T>(value, this.Count);
            this.nodes.Add(node);

            // Restore the heap order
            this.MoveUp(node);
            return node;
        }

        /// <summary>
        /// Removes an arbitrary node from the heap.
        /// </summary>
        /// <param name="node">The node to remove from the heap.</param>
        public override T Remove(ArrayHeapNode<T> node)
        {
            // The idea is to replace the specified node by the very last
            // node and shorten the array by one.
             
            var lastNode = this.nodes[this.Count - 1];
            this.nodes.RemoveAt(lastNode.Index);

            // In case we wanted to remove the node that was the last one,
            // we are done.

            if (node == lastNode)
                return node.Value;

            // Our last node was erased from the array and needs to be
            // inserted again. Of course, we will overwrite the node we
            // wanted to remove. After that operation, we will need
            // to restore the heap property (in general).

            var relation = this.Comparer.Compare(lastNode.Value, node.Value);
            this.PutAt(lastNode, node.Index);

            if (relation < 0)
                this.MoveUp(lastNode);
            else
                this.MoveDown(lastNode);

            return node.Value;
        }

        /// <summary>
        /// Updates the value contained in the specified node.
        /// </summary>
        /// <param name="node">The node to update.</param>
        /// <param name="value">The new value for the node.</param>
        public override void Update(ArrayHeapNode<T> node, T value)
        {
            var relation = this.Comparer.Compare(value, node.Value);
            node.Value = value;

            if (relation < 0)
                this.MoveUp(node);
            else
                this.MoveDown(node);
        }

        /// <summary>
        /// Puts a node at the specified index.
        /// </summary>
        private void PutAt(ArrayHeapNode<T> node, int index)
        {
            node.Index = index;
            this.nodes[index] = node;
        }

        /// <summary>
        /// Merges this heap with the elements of another heap.
        /// </summary>
        public override void Meld(ArrayHeap<T> other)
        {
            this.nodes.AddRange(other.nodes);
            this.Heapify();
        }

        /// <summary>
        /// Determines whether one object should be extracted from the heap earlier
        /// than the second one.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ShouldBeExtractedEarlier(T first, T second)
        {
            return this.Comparer.Compare(first, second) < 0;
        }

        /// <summary>
        /// Gets the index of an element's parent.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetParentIndex(int index) => (index - 1) / this.Arity;

        /// <summary>
        /// Gets the index of the first child of an element.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFirstChildIndex(int index) => this.Arity * index + 1;

        /// <summary>
        /// Converts an unordered list into a heap.
        /// </summary>
        private void Heapify()
        {
            // Leaves of the tree are in fact 1-element heaps, for which there
            // is no need to correct them. The heap property needs to be restored
            // only for higher nodes, starting from the first node that has children.
            // It is the parent of the very last element in the array.

            var lastElementIndex = this.nodes.Count - 1;
            var lastParentWithChildren = this.GetParentIndex(lastElementIndex);

            for (var index = lastParentWithChildren; index >= 0; --index)
                this.MoveDown(this.nodes[index]);
        }

        /// <summary>
        /// Moves a node up in the tree to restore heap order.
        /// </summary>
        /// <param name="node">The node to be moved up.</param>
        private void MoveUp(ArrayHeapNode<T> node)
        {
            var i = node.Index;

            // Instead of swapping items all the way to the root, we will perform
            // a similar optimization as in the insertion sort.

            while (i > 0)
            {
                var parentIndex = this.GetParentIndex(i);
                var parent = this.nodes[parentIndex];

                if (this.ShouldBeExtractedEarlier(node.Value, parent.Value))
                {
                    this.PutAt(parent, i);
                    i = parentIndex;
                }
                else
                    break;
            }

            this.PutAt(node, i);
        }

        /// <summary>
        /// Moves a node down in the tree to restore heap order.
        /// </summary>
        /// <param name="node">The node to be moved down.</param>
        private void MoveDown(ArrayHeapNode<T> node)
        {
            // The node to move down will not actually be swapped every time.
            // Rather, values on the affected path will be moved up, thus leaving a free spot
            // for this value to drop in. Similar optimization as in the insertion sort.
            var index = node.Index;

            int i;
            while ((i = this.GetFirstChildIndex(index)) < this.Count)
            {
                // Check if the current node (pointed by 'index') should really be extracted 
                // first, or maybe one of its children should be extracted earlier.
                var topChild = this.nodes[i];
                var childrenIndexesLimit = Math.Min(i + this.Arity, this.Count);

                while (++i < childrenIndexesLimit)
                {
                    var child = this.nodes[i];
                    if (this.Comparer.Compare(child.Value, topChild.Value) < 0)
                        topChild = child;
                }

                // In case no child needs to be extracted earlier than the current node,
                // there is nothing more to do - the right spot was found.
                if (this.Comparer.Compare(node.Value, topChild.Value) <= 0)
                    break;

                // Move the top child up by one node and now investigate the
                // node that was considered to be the top child (recursive).
                var topChildIndex = topChild.Index;
                this.PutAt(topChild, index);

                index = topChildIndex;
            }

            this.PutAt(node, index);
        }
    }
}