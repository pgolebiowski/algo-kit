using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    public interface IHeap<TValue, THandle>
    {
        IComparer<TValue> Comparer { get; }

        int Count { get; }

        bool IsEmpty { get; }

        TValue Top();

        TValue Pop();

        THandle Add(TValue value);

        TValue Remove(THandle handle);

        void Update(THandle handle, TValue value);
    }
}