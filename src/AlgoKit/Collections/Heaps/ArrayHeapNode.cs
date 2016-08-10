namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of an <see cref="ArrayHeap{T}"/>.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the array heap.</typeparam>
    public class ArrayHeapNode<T> : IHeapHandle<T>
    {
        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// The index of the node within an <see cref="ArrayHeap{T}"/>.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayHeapNode{T}"/> class,
        /// containing the specified value.
        /// </summary>
        /// <param name="value">The value to contain in the <see cref="ArrayHeapNode{T}"/>.</param>
        /// <param name="index">The index of the node within an <see cref="ArrayHeap{T}"/>.</param>
        public ArrayHeapNode(T value, int index)
        {
            this.Value = value;
            this.Index = index;
        }
    }
}