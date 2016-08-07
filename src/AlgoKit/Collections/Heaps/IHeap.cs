using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a specialized tree-based data structure capable
    /// of retrieving the top element efficiently.
    /// </summary>
    public interface IHeap<TValue>
    {
        IComparer<TValue> Comparer { get; }

        int Count { get; }

        bool IsEmpty { get; }

        TValue Top();

        TValue Pop();

        void Add(TValue value);
    }
}