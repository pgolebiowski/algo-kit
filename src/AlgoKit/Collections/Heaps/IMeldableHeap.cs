namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap supporting a meld operation.
    /// </summary>
    public interface IMeldableHeap<TValue, in TOtherHeap> : IHeap<TValue>
    {
        void Meld(TOtherHeap other);
    }
}