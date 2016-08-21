namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap node. It is a wrapper around a key and a value.
    /// </summary>
    public interface IHeapNode<out TKey, out TValue>
    {
        /// <summary>
        /// Gets the key for the value.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// Gets the value contained in the node.
        /// </summary>
        TValue Value { get; }
    }
}