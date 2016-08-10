using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a specialized tree-based data structure capable
    /// of retrieving the top element efficiently. The heap is meldable
    /// and supports access to the elements via handles.
    /// </summary>
    public interface IHeap<T>
    {
        IComparer<T> Comparer { get; }

        int Count { get; }

        bool IsEmpty { get; }

        T Top();

        T Pop();

        IHeapHandle<T> Add(T value);

        T Remove(IHeapHandle<T> handle);

        void Update(IHeapHandle<T> handle, T value);

        void Meld(IHeap<T> other);
    }

    public interface IHeap<TValue, THandle, in THeap> : IHeap<TValue>
        where THandle : class, IHeapHandle<TValue>
        where THeap : class, IHeap<TValue, THandle, THeap>
    {
        new THandle Add(TValue value);

        TValue Remove(THandle handle);

        void Update(THandle handle, TValue value);

        void Meld(THeap other);
    }
}