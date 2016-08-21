using AlgoKit.Collections.Heaps;

namespace AlgoKit.Test.Collections.Heaps
{
    public class HandleKeyPair
    {
        public HandleKeyPair(IHeapNode<int, string> node, int key)
        {
            this.Node = node;
            this.Key = key;
        }

        public IHeapNode<int, string> Node { get; }
        public int Key { get; set; }
    }
}