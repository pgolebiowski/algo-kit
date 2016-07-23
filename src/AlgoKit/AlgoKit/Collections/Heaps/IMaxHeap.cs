using System;

namespace AlgoKit.Collections.Heaps
{
    public interface IMaxHeap<T> where T : IComparable<T>
    {
        T Maximum { get; }

        T ExtractMax();

        void Insert(T item);

        void Update(int index, T item);

        void DeleteMax();
    }
}
