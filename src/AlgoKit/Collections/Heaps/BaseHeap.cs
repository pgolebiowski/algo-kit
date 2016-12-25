using System;
using System.Collections;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    public abstract class BaseHeap<TKey, TValue, TNode, THeap> 
        : IHeap<TKey, TValue, TNode, THeap>
        where TNode : class, IHeapNode<TKey, TValue>
        where THeap : class, IHeap<TKey, TValue, TNode, THeap>
    {
        public IComparer<TKey> Comparer { get; protected set; }

        public abstract int Count { get; }

        public bool IsEmpty => this.Count == 0;

        public abstract TNode Peek();

        public abstract TNode Pop();

        public abstract TNode Add(TKey key, TValue value);

        public abstract TValue Remove(TNode node);

        public abstract void Update(TNode node, TKey key);

        public abstract void Merge(THeap other);

        IHeapNode<TKey, TValue> IHeap<TKey, TValue>.Peek() => this.Peek();

        IHeapNode<TKey, TValue> IHeap<TKey, TValue>.Pop() => this.Pop();

        IHeapNode<TKey, TValue> IHeap<TKey, TValue>.Add(TKey key, TValue value) => this.Add(key, value);

        public TValue Remove(IHeapNode<TKey, TValue> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var casted = node as TNode;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(TNode), node.GetType()));

            return this.Remove(casted);
        }

        public void Update(IHeapNode<TKey, TValue> node, TKey key)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var casted = node as TNode;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(TNode), node.GetType()));

            this.Update(casted, key);
        }

        public void Merge(IHeap<TKey, TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var casted = other as THeap;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(THeap), other.GetType()));

            this.Merge(casted);
        }

        private static string FormatTypeMismatchMessage(Type expected, Type actual)
        {
            return $"Expected type '{expected.FullName}', but was '{actual.FullName}'.";
        }

        public abstract IEnumerator<IHeapNode<TKey, TValue>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}