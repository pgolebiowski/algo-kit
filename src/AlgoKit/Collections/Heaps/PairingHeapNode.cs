namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of a pairing heap. It is a part of a doubly linked list
    /// of its siblings and a parent of some other node.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the pairing heap.</typeparam>
    public class PairingHeapNode<T>
    {
        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the previous node in the list of this node's siblings.
        /// In case this node is the leftmost node in the list, the previous node
        /// is its parent node. 
        /// </summary>
        public PairingHeapNode<T> Previous { get; set; }

        /// <summary>
        /// Gets or sets the next node in the list of this node's siblings.
        /// </summary>
        public PairingHeapNode<T> Next { get; set; }

        /// <summary>
        /// Gets or sets the leftmost child of this node.
        /// </summary>
        public PairingHeapNode<T> Child { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PairingHeapNode{T}"/> class,
        /// containing the specified value.
        /// </summary>
        /// <param name="value">The value to contain in the <see cref="PairingHeapNode{T}"/>.</param>
        public PairingHeapNode(T value)
        {
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
    }
}