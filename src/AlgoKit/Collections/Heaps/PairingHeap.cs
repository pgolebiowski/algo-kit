using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a self-adjusting heap-ordered multiary tree inspired by
    /// binomial trees and splay trees.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the pairing heap.</typeparam>
    public class PairingHeap<T> : IHeap<T, PairingHeapNode<T>>
    {
        /// <summary>
        /// Creates an empty pairing heap.
        /// </summary>
        /// <param name="comparer">
        /// The comparer used to determine whether one object should be extracted
        /// from the heap earlier than the other one.
        /// </param>
        public PairingHeap(IComparer<T> comparer)
        {
            this.Comparer = comparer;
        }

        /// <summary>
        /// Gets the root node of the pairing heap.
        /// </summary>
        public PairingHeapNode<T> Root { get; private set; }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> for the pairing heap.
        /// </summary>
        public IComparer<T> Comparer { get; }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Returns true if the heap is empty, false otherwise.
        /// </summary>
        public bool IsEmpty => this.Count == 0;

        /// <summary>
        /// Gets the top element of the heap.
        /// </summary>
        public T Top()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            return this.Root.Value;
        }

        /// <summary>
        /// Returns the top element after removing it from the heap.
        /// </summary>
        public T Pop()
        {
            // Removing the root leaves us with a collection of heap-ordered trees,
            // We combine all these trees by pairwise merging to form one new tree.
            // However, the order in which we combine the trees is important.

            return this.Remove(this.Root);
        }

        /// <summary>
        /// Adds an object to the heap.
        /// </summary>
        public PairingHeapNode<T> Add(T item)
        {
            // Create a one-node tree for the specified item and merge it with this heap.
            var tree = new PairingHeapNode<T>(item);

            this.Root = this.Merge(this.Root, tree);
            ++this.Count;

            return tree;
        }

        /// <summary>
        /// Removes an arbitrary node from the heap.
        /// </summary>
        public T Remove(PairingHeapNode<T> node)
        {
            // Remove the node from its list of siblings. Merge all its children to form
            // a new tree and merge that tree with the root.

            var item = node.Value;

            if (node == this.Root)
            {
                // Simplified case when we remove the root
                this.Root = this.MergePairwisely(node.Child);
            }
            else
            {
                // The node is somewhere in the middle of the tree, so before we handle
                // the child of the node, we also need to fix the list of siblings.

                node.RemoveFromListOfSiblings();

                // The only part left is the child of the node. As stated previously,
                // we will simply merge it with the entire heap.

                this.Root = this.Merge(this.Root, this.MergePairwisely(node.Child));
            }

            --this.Count;
            return item;
        }

        /// <summary>
        /// Updates the value contained in the specified node.
        /// </summary>
        /// <param name="node">The node to update.</param>
        /// <param name="value">The new value for the node.</param>
        public void Update(PairingHeapNode<T> node, T value)
        {
            var relation = this.Comparer.Compare(value, node.Value);
            node.Value = value;

            // If the new value is considered equal to the previous value, there is no need
            // to fix the heap property, because it is already preserved.

            if (relation == 0)
                return;

            // Although the algorithm for updating an arbitrary node in the middle of the
            // heap is the same no matter whether we increase or decrease the key, the
            // way of handling the root differs.

            if (node == this.Root)
            {
                // In case the root gets a value that should be extracted from the heap even
                // earlier, there is also no need to fix anything.

                if (relation < 0)
                    return;

                // Otherwise, the root needs to go somewhere down. It needs to be replaced
                // with one of its children. Thus, simply perform pop and add the new value.

                this.Pop();
                this.Add(value);
                return;
            }

            // Here, we're handling the case when the node to be updated is somewhere
            // in the middle of the heap. The idea is to simply cut the node from its
            // list of siblings and to merge it with the root.

            node.RemoveFromListOfSiblings();
            this.Root = this.Merge(this.Root, node);
        }

        /// <summary>
        /// Merges this heap with another pairing heap.
        /// </summary>
        /// <param name="other">The heap to be merged with this heap.</param>
        public void Merge(PairingHeap<T> other)
        {
            this.Count += other.Count;
            this.Root = this.Merge(this.Root, other.Root);
        }

        /// <summary>
        /// Merges two heaps and returns the root of the resulting heap.
        /// </summary>
        private PairingHeapNode<T> Merge(PairingHeapNode<T> a, PairingHeapNode<T> b)
        {
            // If merging occurs between a non-empty pairing heap and an empty
            // pairing heap, merge just returns the non-empty pairing heap.

            if (a == null)
                return b;

            if (b == null)
                return a;

            if (a == b)
                return a;

            // If both pairing heaps are non-empty, the merge function returns
            // a new heap where the smallest root of the two heaps is the root of 
            // the new combined heap and adds the other heap to the list of its children.

            PairingHeapNode<T> parent, child;

            if (this.Comparer.Compare(a.Value, b.Value) < 0)
            {
                parent = a;
                child = b;
            }
            else
            {
                parent = b;
                child = a;
            }

            // The smallest root will be the new leftmost child of the largest root.
            // Thus, add it to the list of siblings and make a parent-child relation.

            child.Next = parent.Child;
            if (parent.Child != null)
                parent.Child.Previous = child;

            child.Previous = parent;
            parent.Child = child;

            // Roots have no siblings, so adjust the pointers accordingly.
            parent.Next = null;
            parent.Previous = null;

            return parent;
        }

        /// <summary>
        /// Performs a two-pass pairing over a list of siblings to form a single tree. 
        /// </summary>
        /// <param name="node">The leftmost node (head) of the list to combine.</param>
        private PairingHeapNode<T> MergePairwisely(PairingHeapNode<T> node)
        {
            if (node == null)
                return null;

            PairingHeapNode<T> result, tail = null, next = node;

            // The first pass is based on iterating left to right, merging trees pairwisely.
            // We take two trees from the list, replace them by the result of merging, and
            // consider two next trees. Note that after a pair of trees is merged, the formed
            // tree is left for later. After this pass we will have ceiling(size / 2) trees.

            // On the second pass, we will iterate back, so on the first pass we will maintain 
            // a temporary list structure for iterating through (during the second pass).

            while (next != null)
            {
                // Take two trees from the list
                var a = next;
                var b = a.Next;

                // Is there a tree to merge 'a' with?
                if (b != null)
                {
                    next = b.Next;
                    result = this.Merge(a, b);

                    // Maintain our temporary list for iterating in the second pass
                    result.Previous = tail;
                    tail = result;
                }
                else
                {
                    a.Previous = tail;
                    tail = a;
                    break;
                }
            }

            // On the second pass, iterate back (move right to left). Every time replace
            // the rightmost tree with the result of merging it with its predecessor.

            result = null;
            while (tail != null)
            {
                next = tail.Previous;
                result = this.Merge(result, tail);
                tail = next;
            }

            // Return a reference to the root of the resulting tree
            return result;
        }
    }
}
