using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of a binomial heap. Binomial trees are encoded as
    /// left-child right-sibling binary trees.
    /// </summary>
    public class BinomialHeapNode<TKey, TValue> : IHeapNode<TKey, TValue>
    {
        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Key"/>
        public TKey Key { get; internal set; }

        /// <inheritdoc cref="IHeapNode{TKey,TValue}.Value"/>
        public TValue Value { get; }

        /// <summary>
        /// Gets the rank of the binomial tree.
        /// </summary>
        public int Rank { get; internal set; }

        /// <summary>
        /// Gets or sets the parent node.
        /// </summary>
        public BinomialHeapNode<TKey, TValue> Parent { get; internal set; }

        /// <summary>
        /// Gets or sets the first child.
        /// </summary>
        public BinomialHeapNode<TKey, TValue> Left { get; internal set; }

        /// <summary>
        /// Gets or sets the next sibling.
        /// </summary>
        public BinomialHeapNode<TKey, TValue> Right { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinomialHeapNode{TKey,TValue}"/> class,
        /// containing the specified value.
        /// </summary>
        public BinomialHeapNode(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// Traverses this heap, visiting each node exactly once.
        /// </summary>
        public IEnumerable<BinomialHeapNode<TKey, TValue>> Traverse()
        {
            yield return this;

            for (var child = this.Left; child != null; child = child.Right)
                foreach (var node in child.Traverse())
                    yield return node;
        }
    }
}