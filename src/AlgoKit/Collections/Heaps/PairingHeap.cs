using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a self-adjusting heap-ordered multiary tree inspired by
    /// binomial trees and splay trees.
    /// </summary>
    public class PairingHeap<TKey, TValue> 
        : BaseHeap<TKey, TValue, PairingHeapNode<TKey, TValue>, PairingHeap<TKey, TValue>>
    {
        private int count;

        /// <summary>
        /// Creates an empty pairing heap.
        /// </summary>
        /// <param name="comparer">
        /// The comparer used to determine whether one object should be extracted
        /// from the heap earlier than the other one.
        /// </param>
        public PairingHeap(IComparer<TKey> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            this.Comparer = comparer;
        }

        /// <summary>
        /// Gets the root node of the pairing heap.
        /// </summary>
        public PairingHeapNode<TKey, TValue> Root { get; private set; }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Count"/>
        public override int Count => this.count;

        /// <inheritdoc cref="IHeap{TKey,TValue}.Peek"/>
        public override PairingHeapNode<TKey, TValue> Peek()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            return this.Root;
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Pop"/>
        public override PairingHeapNode<TKey, TValue> Pop()
        {
            // Removing the root leaves us with a collection of heap-ordered trees,
            // We combine all these trees by pairwise merging to form one new tree.
            // However, the order in which we combine the trees is important.

            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            var result = this.Root;
            this.Remove(this.Root);
            return result;
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Add"/>
        public override PairingHeapNode<TKey, TValue> Add(TKey key, TValue value)
        {
            // Create a one-node tree for the specified item and merge it with this heap.
            var tree = new PairingHeapNode<TKey, TValue>(key, value);

            this.Root = this.Merge(this.Root, tree);
            ++this.count;

            return tree;
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Remove"/>
        public override TValue Remove(PairingHeapNode<TKey, TValue> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

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

            --this.count;
            return item;
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Update"/>
        public override void Update(PairingHeapNode<TKey, TValue> node, TKey key)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var relation = this.Comparer.Compare(key, node.Key);
            node.Key = key;

            // If the new value is considered equal to the previous value, there is no need
            // to fix the heap property, because it is already preserved.

            if (relation == 0)
                return;

            if (relation < 0)
            {
                // In case the root gets a value that should be extracted from the heap even
                // earlier, there is also no need to fix anything.

                if (node == this.Root)
                    return;

                node.RemoveFromListOfSiblings();
                this.Root = this.Merge(this.Root, node);
                return;
            }

            // In case the node was updated with a greater value, it is in the right spot.
            // However, the heap property might be violated for all its children. For that
            // reason we will merge them pairwisely to form a single tree and merge this
            // tree with our heap.

            var child = node.Child;

            if (child == null)
                return;

            node.Child = null;
            child.Previous = null; // TODO: this line might be not needed

            var tree = this.MergePairwisely(child);
            this.Root = this.Merge(this.Root, tree);
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.Merge"/>
        public override void Merge(PairingHeap<TKey, TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            this.count += other.count;
            this.Root = this.Merge(this.Root, other.Root);
        }

        /// <inheritdoc cref="IHeap{TKey,TValue}.GetEnumerator"/>
        public override IEnumerator<IHeapNode<TKey, TValue>> GetEnumerator()
        {
            return this.IsEmpty
                ? Enumerable.Empty<IHeapNode<TKey, TValue>>().GetEnumerator()
                : this.Root.Traverse().GetEnumerator();
        }

        /// <summary>
        /// Merges two heaps and returns the root of the resulting heap.
        /// </summary>
        private PairingHeapNode<TKey, TValue> Merge(PairingHeapNode<TKey, TValue> a, PairingHeapNode<TKey, TValue> b)
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

            PairingHeapNode<TKey, TValue> parent, child;

            if (this.Comparer.Compare(a.Key, b.Key) < 0)
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
        private PairingHeapNode<TKey, TValue> MergePairwisely(PairingHeapNode<TKey, TValue> node)
        {
            if (node == null)
                return null;

            PairingHeapNode<TKey, TValue> result, tail = null, next = node;

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
