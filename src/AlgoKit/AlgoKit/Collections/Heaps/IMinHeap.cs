using System;

namespace AlgoKit.Collections.Heaps
{
    public interface IMinHeap<T> where T : IComparable<T>
    {
        T Minimum { get; }

        T ExtractMin();

        void Insert(T item);

        void Update(int index, T item);

        void DeleteMin();
    }
}
