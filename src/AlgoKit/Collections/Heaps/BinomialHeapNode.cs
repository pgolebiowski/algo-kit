namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of a binomial heap. Binomial trees are encoded as
    /// left-child right-sibling binary trees.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the binomial heap.</typeparam>
    public class BinomialHeapNode<T>
    {
        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets the rank of the binomial tree.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public BinomialHeapNode<T> Parent { get; set; }

        /// <summary>
        /// Gets or sets the first child.
        /// </summary>
        public BinomialHeapNode<T> Left { get; set; }

        /// <summary>
        /// Gets or sets the next sibling.
        /// </summary>
        public BinomialHeapNode<T> Right { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinomialHeapNode{T}"/> class,
        /// containing the specified value.
        /// </summary>
        /// <param name="value">The value to contain in the <see cref="BinomialHeapNode{T}"/>.</param>
        public BinomialHeapNode(T value)
        {
            this.Value = value;
        }
    }
}