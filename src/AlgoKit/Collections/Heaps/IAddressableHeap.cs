namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a meldable heap supporting access to the elements of the heap
    /// via handles. It allows elements be removed or updated.
    /// </summary>
    public interface IAddressableHeap<TValue, THandle, in TOtherHeap>
        : IMeldableHeap<TValue, TOtherHeap>
    {
        new THandle Add(TValue value);

        TValue Remove(THandle handle);

        void Update(THandle handle, TValue value);
    }
}