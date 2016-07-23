using System;

namespace AlgoKit.Test.Collections.Heaps
{
    public class PairWithPriority<TKey, TValue> : IComparable<PairWithPriority<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public PairWithPriority(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public int CompareTo(PairWithPriority<TKey, TValue> other)
        {
            return this.Key.CompareTo(other.Key);
        }
    }
}