using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Collections.Heaps;
using NUnit.Framework;

namespace AlgoKit.Test.Collections.Heaps
{
    [TestFixture]
    public class PairingHeapTests
    {
        [Test]
        public void Should_properly_detect_top_element()
        {
            // Arrange
            var heap = new PairingHeap<int>(Comparer<int>.Default);
            var collection = Utils.GenerateList(50000, -5000, 5000).OrderByDescending(x => x);

            // Act && Assert
            foreach (var element in collection)
            {
                heap.Add(element);
                Assert.AreEqual(element, heap.Top());
            }
        }

        [Test]
        public void Should_not_allow_getting_or_extracting_top_from_empty_heap()
        {
            // Arrange
            var heap = new PairingHeap<int>(Comparer<int>.Default);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.Top());
            Assert.Throws<InvalidOperationException>(() => heap.Top());
        }

        [Test]
        public void Elements_should_be_added_and_popped_correctly()
        {
            // Arrange
            var heap = new PairingHeap<int>(Comparer<int>.Default);
            var items = Utils.GenerateList(10000, -1000, 1000);
            var count = 0;

            // Act
            foreach (var item in items)
            {
                Assert.AreEqual(count, heap.Count);
                heap.Add(item);
                Assert.AreEqual(++count, heap.Count);
            }

            // Assert
            foreach (var item in items.OrderBy(x => x))
            {
                Assert.AreEqual(false, heap.IsEmpty);
                Assert.AreEqual(item, heap.Top());

                Assert.AreEqual(count, heap.Count);
                Assert.AreEqual(item, heap.Pop());
                Assert.AreEqual(--count, heap.Count);
            }

            Assert.AreEqual(true, heap.IsEmpty);
        }

        private class HandleValuePair
        {
            public HandleValuePair(PairingHeapNode<int> handle, int value)
            {
                this.Handle = handle;
                this.Value = value;
            }

            public PairingHeapNode<int> Handle { get; }
            public int Value { get; set; }
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(3, 3)]
        [TestCase(8, 10)]
        [TestCase(10, int.MaxValue)]
        public void Elements_should_be_updated_correctly(int heapSize, int valueLimit)
        {
            for (var seed = 0; seed < 15; ++seed)
            {
                // Arrange
                var heap = new PairingHeap<int>(Comparer<int>.Default);

                var handles = new List<HandleValuePair>();
                var random = new Random(seed);

                for (var i = 0; i < heapSize; ++i)
                {
                    var value = random.Next(valueLimit);
                    var handle = heap.Add(value);
                    handles.Add(new HandleValuePair(handle, value));
                }

                // Act
                for (var i = 0; i < 150000; ++i)
                {
                    var handleIndex = random.Next(0, heapSize);
                    var newValue = random.Next(valueLimit);
                    var handle = handles[handleIndex];

                    heap.Update(handle.Handle, newValue);
                    handles[handleIndex].Value = newValue;

                    // Assert
                    var top = handles.Select(x => x.Value).Min();
                    Assert.AreEqual(top, heap.Top());
                }
            }
        }
    }
}