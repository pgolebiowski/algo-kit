using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    public abstract class BaseHeap<TValue, THandle, THeap> : IHeap<TValue, THandle, THeap>
        where THandle : class, IHeapHandle<TValue>
        where THeap : class, IHeap<TValue, THandle, THeap>
    {
        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> for the heap.
        /// </summary>
        public IComparer<TValue> Comparer { get; protected set; }

        public abstract int Count { get; }

        /// <summary>
        /// Returns true if the heap is empty, false otherwise.
        /// </summary>
        public bool IsEmpty => this.Count == 0;

        public abstract TValue Top();

        public abstract TValue Pop();

        public abstract THandle Add(TValue value);

        public abstract TValue Remove(THandle handle);

        public abstract void Update(THandle handle, TValue value);

        public abstract void Meld(THeap other);

        IHeapHandle<TValue> IHeap<TValue>.Add(TValue value) => this.Add(value);

        public TValue Remove(IHeapHandle<TValue> handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            var casted = handle as THandle;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(THandle), handle?.GetType()));

            return this.Remove(casted);
        }

        public void Update(IHeapHandle<TValue> handle, TValue value)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            var casted = handle as THandle;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(THandle), handle?.GetType()));

            this.Update(casted, value);
        }

        public void Meld(IHeap<TValue> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            var casted = other as THeap;
            if (casted == null)
                throw new ArgumentException(FormatTypeMismatchMessage(typeof(THeap), other?.GetType()));

            this.Meld(casted);
        }

        private static string FormatTypeMismatchMessage(Type expected, Type actual)
        {
            return $"Expected type '{expected.FullName}', but was '{actual.FullName}'.";
        }
    }
}