using AlgoKit.Collections.Heaps;

namespace AlgoKit.Test.Collections.Heaps
{
    public class HandleValuePair
    {
        public HandleValuePair(IHeapHandle<int> handle, int value)
        {
            this.Handle = handle;
            this.Value = value;
        }

        public IHeapHandle<int> Handle { get; }
        public int Value { get; set; }
    }
}