using System;
using System.Diagnostics;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a binomial tree.
    /// </summary>
    [DebuggerDisplay("Root = {Root.Value}, Order = {Order}, Count = {Count}")]
    internal class BinomialTree<T> where T : IComparable<T>
    {
        public BinomialTreeNode<T> Root { get; }

        public int Count => 1 << this.Order;

        public int Order { get; private set; }
        
        public BinomialTree(T element)
        {
            this.Root = new BinomialTreeNode<T>(element);
            this.Order = 0;
        }

        public BinomialTree(BinomialTreeNode<T> root, int order)
        {
            this.Root = root;
            this.Order = order;
        }

        public BinomialTree<T> Merge(BinomialTree<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.Order != this.Order)
                throw new ArgumentException($"Expected tree order to be '{this.Order}', but was '{other.Order}'.");

            if (this.Root.Value.CompareTo(other.Root.Value) <= 0)
            {
                ++this.Order;
                this.Root.AddLast(other.Root);
                return this;
            }
            else
            {
                ++other.Order;
                other.Root.AddLast(this.Root);
                return other;
            }
        } 
    }
}