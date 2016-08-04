using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a node of a binomial tree.
    /// </summary>
    [DebuggerDisplay("Value = {Value}")]
    public class BinomialTreeNode<T> : LinkedList<BinomialTreeNode<T>>
    {
        public T Value { get; }

        public BinomialTreeNode(T value)
        {
            this.Value = value;
        }
    }
}