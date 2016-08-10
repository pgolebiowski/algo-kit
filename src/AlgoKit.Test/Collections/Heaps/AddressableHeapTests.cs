using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Collections.Heaps;
using MoreLinq;
using NUnit.Framework;

namespace AlgoKit.Test.Collections.Heaps
{
    /// <summary>
    /// Tests for heaps that implement the IAddressableHeap interface.
    /// </summary>
    [TestFixture(typeof(ArrayHeap<int>))]
    [TestFixture(typeof(PairingHeap<int>))]
    [TestFixture(typeof(BinomialHeap<int>))]
    public class AddressableHeapTests<THeap>
    {
        private static dynamic CreateHeapInstance()
        {
            return Activator.CreateInstance(typeof(THeap), Comparer<int>.Default);
        }

        public class HeapConfiguration
        {
            public int HeapSize { get; }
            public int ValueLimit { get; }

            public HeapConfiguration(int heapSize, int valueLimit)
            {
                this.HeapSize = heapSize;
                this.ValueLimit = valueLimit;
            }

            public List<int> GenerateValues(Random random)
            {
                return Enumerable.Range(1, this.HeapSize)
                    .Select(x => random.Next(this.ValueLimit))
                    .ToList();
            }
        }

        public static IEnumerable<HeapConfiguration> GetHeapConfigurations()
        {
            yield return new HeapConfiguration(1, 1);

            for (var i = 1; i <= 10; ++i)
                yield return new HeapConfiguration(i, 2*i + 1);

            yield return new HeapConfiguration(30, int.MaxValue);
        }

        private static int Top(IEnumerable<HandleValuePair> list) => list.Select(x => x.Value).Min();

        [TestCaseSource(nameof(GetHeapConfigurations))]
        public void Top_should_be_properly_maintained_after_addition(HeapConfiguration conf)
        {
            for (var seed = 0; seed < 15000; ++seed)
            {
                // Arrange
                var values = conf.GenerateValues(new Random(seed));
                var heap = CreateHeapInstance();
                var top = int.MaxValue;
                var count = 0;

                foreach (var value in values)
                {
                    if (value < top)
                        top = value;

                    // Act
                    heap.Add(value);
                    ++count;

                    // Assert
                    Assert.AreEqual(top, heap.Top());
                    Assert.AreEqual(count, heap.Count);
                }
            }
        }

        [Test]
        public void Should_not_allow_top_or_pop_from_empty_heap()
        {
            // Arrange
            var heap = CreateHeapInstance();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.Top());
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
        }

        [TestCaseSource(nameof(GetHeapConfigurations))]
        public void Elements_should_be_popped_correctly(HeapConfiguration conf)
        {
            for (var seed = 0; seed < 5000; ++seed)
            {
                // Arrange
                var values = conf.GenerateValues(new Random(seed));
                var heap = CreateHeapInstance();
                var count = 0;

                foreach (var value in values)
                {
                    Assert.AreEqual(count, heap.Count);
                    heap.Add(value);
                    Assert.AreEqual(++count, heap.Count);
                }

                // Act & Assert
                foreach (var value in values.OrderBy(x => x))
                {
                    Assert.AreEqual(false, heap.IsEmpty);
                    Assert.AreEqual(value, heap.Top());

                    Assert.AreEqual(count, heap.Count);
                    Assert.AreEqual(value, heap.Pop());
                    Assert.AreEqual(--count, heap.Count);
                }

                Assert.AreEqual(true, heap.IsEmpty);
            }
        }

        [TestCaseSource(nameof(GetHeapConfigurations))]
        public void Elements_should_be_updated_correctly(HeapConfiguration conf)
        {
            for (var seed = 0; seed < 15; ++seed)
            {
                // Arrange
                var random = new Random(seed);
                var values = conf.GenerateValues(random);
                var heap = CreateHeapInstance();

                var handles = values
                    .Select(v => new HandleValuePair(heap.Add(v), v))
                    .ToList();

                for (var i = 0; i < 15000; ++i)
                {
                    var handleIndex = random.Next(0, conf.HeapSize);
                    var newValue = random.Next(conf.ValueLimit);
                    var handle = handles[handleIndex];

                    // Act
                    heap.Update(handle.Handle, newValue);
                    handles[handleIndex].Value = newValue;

                    // Assert
                    Assert.AreEqual(Top(handles), heap.Top());
                }
            }
        }

        [TestCaseSource(nameof(GetHeapConfigurations))]
        public void All_elements_should_be_removed_correctly(HeapConfiguration conf)
        {
            for (var seed = 0; seed < 5000; ++seed)
            {
                // Arrange
                var random = new Random(seed);
                var values = conf.GenerateValues(random);
                var heap = CreateHeapInstance();
                var count = conf.HeapSize;

                var handles = values
                    .Select(v => new HandleValuePair(heap.Add(v), v))
                    .ToList();

                // Act & Assert
                while (!heap.IsEmpty)
                {
                    Assert.AreEqual(Top(handles), heap.Top());
                    Assert.AreEqual(count, heap.Count);

                    var handle = handles[random.Next(count--)];
                    handles.Remove(handle);
                    heap.Remove(handle.Handle);

                    if (count == 0)
                    {
                        Assert.Throws<InvalidOperationException>(() => heap.Top());
                    }
                    else
                    {
                        Assert.AreEqual(Top(handles), heap.Top());
                        Assert.AreEqual(count, heap.Count);
                    }
                }
            }
        }

        [TestCaseSource(nameof(GetHeapConfigurations))]
        public void Elements_should_be_removed_correctly(HeapConfiguration conf)
        {
            for (var seed = 0; seed < 5000; ++seed)
            {
                // Arrange
                var random = new Random(seed);
                var values = conf.GenerateValues(random);

                for (var toRemove = 0; toRemove < conf.HeapSize; ++toRemove)
                {
                    var heap = CreateHeapInstance();
                    var count = conf.HeapSize;

                    var handles = values
                        .Select(v => new HandleValuePair(heap.Add(v), v))
                        .ToList();

                    // Act & Assert

                    // Part I - remove one node
                    Assert.AreEqual(Top(handles), heap.Top());
                    Assert.AreEqual(count--, heap.Count);

                    heap.Remove(handles[toRemove].Handle);
                    handles.RemoveAt(toRemove);

                    if (count == 0)
                    {
                        Assert.Throws<InvalidOperationException>(() => heap.Top());
                    }
                    else
                    {
                        Assert.AreEqual(Top(handles), heap.Top());
                        Assert.AreEqual(count, heap.Count);
                    }

                    // Part II - pop until empty
                    while (!heap.IsEmpty)
                    {
                        var min = handles.MinBy(x => x.Value);
                        handles.Remove(min);

                        Assert.AreEqual(min.Value, heap.Pop());
                        Assert.AreEqual(--count, heap.Count);
                    }

                    Assert.True(heap.IsEmpty);
                    Assert.AreEqual(0, count);
                }
            }
        }
    }
}