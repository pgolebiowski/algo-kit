using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a specialized tree-based collection of key/value
    /// pairs capable of retrieving the top element efficiently.
    /// </summary>
    public interface IHeap<TKey, TValue> : IEnumerable<IHeapNode<TKey, TValue>>
    {
        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> for the heap.
        /// </summary>
        IComparer<TKey> Comparer { get; }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns true if the heap is empty, false otherwise.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns the object at the top of the heap without removing it.
        /// </summary>
        IHeapNode<TKey, TValue> Peek();

        /// <summary>
        /// Returns the top element after removing it from the heap.
        /// </summary>
        IHeapNode<TKey, TValue> Pop();

        /// <summary>
        /// Adds an element with the specified key and value into the heap.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        IHeapNode<TKey, TValue> Add(TKey key, TValue value);

        /// <summary>
        /// Removes an arbitrary element from the heap.
        /// </summary>
        /// <param name="node">The handle of the element to remove from the heap.</param>
        TValue Remove(IHeapNode<TKey, TValue> node);

        /// <summary>
        /// Updates the key contained in the specified node.
        /// </summary>
        /// <param name="node">The node of the element to update.</param>
        /// <param name="key">The new key for the element.</param>
        void Update(IHeapNode<TKey, TValue> node, TKey key);

        /// <summary>
        /// Merges this heap with the elements of another heap.
        /// </summary>
        /// <param name="other">The heap to be merged with this heap.</param>
        void Merge(IHeap<TKey, TValue> other);
    }

    public interface IHeap<TKey, TValue, TNode, in THeap> : IHeap<TKey, TValue>
        where TNode : class, IHeapNode<TKey, TValue>
        where THeap : class, IHeap<TKey, TValue, TNode, THeap>
    {
        new TNode Peek();

        new TNode Pop();

        new TNode Add(TKey key, TValue value);

        TValue Remove(TNode node);

        void Update(TNode handle, TKey key);

        void Merge(THeap other);
    }
}