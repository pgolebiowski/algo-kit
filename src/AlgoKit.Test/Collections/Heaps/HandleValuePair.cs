namespace AlgoKit.Test.Collections.Heaps
{
    public class HandleValuePair
    {
        public HandleValuePair(dynamic handle, int value)
        {
            this.Handle = handle;
            this.Value = value;
        }

        public dynamic Handle { get; }
        public int Value { get; set; }
    }
}