namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a heap handle, which allows elements to be removed
    /// or updated. It is a wrapper (often a node) around a value.
    /// </summary>
    public interface IHeapHandle<out T>
    {
        T Value { get; }
    }
}