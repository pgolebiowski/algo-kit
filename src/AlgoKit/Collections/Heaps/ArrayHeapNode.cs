namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of an <see cref="ArrayHeap{TKey,TValue}"/>.
    /// </summary>
    public class ArrayHeapNode<TKey, TValue> : IHeapNode<TKey, TValue>
    {
        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Key"/>
        public TKey Key { get; internal set; }

        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Value"/>
        public TValue Value { get; }

        /// <summary>
        /// The index of the node within an <see cref="ArrayHeap{TKey,TValue}"/>.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayHeapNode{TKey,TValue}"/> class,
        /// containing the specified value.
        /// </summary>
        public ArrayHeapNode(TKey key, TValue value, int index)
        {
            this.Key = key;
            this.Value = value;
            this.Index = index;
        }
    }
}