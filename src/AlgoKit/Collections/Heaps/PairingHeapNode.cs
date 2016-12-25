using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of a pairing heap. It is a part of a doubly linked list
    /// of its siblings and a parent of some other node.
    /// </summary>
    public class PairingHeapNode<TKey, TValue> : IHeapNode<TKey, TValue>
    {
        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Key"/>
        public TKey Key { get; internal set; }

        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Value"/>
        public TValue Value { get; }

        /// <summary>
        /// Gets or sets the previous node in the list of this node's siblings.
        /// In case this node is the leftmost node in the list, the previous node
        /// is its parent node. 
        /// </summary>
        public PairingHeapNode<TKey, TValue> Previous { get; internal set; }

        /// <summary>
        /// Gets or sets the next node in the list of this node's siblings.
        /// </summary>
        public PairingHeapNode<TKey, TValue> Next { get; internal set; }

        /// <summary>
        /// Gets or sets the leftmost child of this node.
        /// </summary>
        public PairingHeapNode<TKey, TValue> Child { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PairingHeapNode{TKey,TValue}"/> class,
        /// containing the specified value.
        /// </summary>
        public PairingHeapNode(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Removes the node from its list of siblings. The child is not affected.
        /// </summary>
        public void RemoveFromListOfSiblings()
        {
            // Depending on whether the node is the leftmost node or not, we should
            // update the 'child' pointer of the parent (for the leftmost node) or the 
            // 'next' pointer of the previous sibling (for other nodes).

            if (this == this.Previous.Child)
                this.Previous.Child = this.Next;
            else
                this.Previous.Next = this.Next;

            // If the node is not the last element in the list of siblings, we also
            // need to fix the 'previous' pointer of the next sibling.

            if (this.Next != null)
                this.Next.Previous = this.Previous;
        }

        /// <summary>
        /// Traverses this heap, visiting each node exactly once.
        /// </summary>
        public IEnumerable<PairingHeapNode<TKey, TValue>> Traverse()
        {
            yield return this;

            for (var child = this.Child; child != null; child = child.Next)
                foreach (var node in child.Traverse())
                    yield return node;
        }
    }
}