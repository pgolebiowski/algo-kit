using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Collections.Heaps;
using NUnit.Framework;

namespace AlgoKit.Test.Collections.Heaps
{
    [TestFixture]
    public class ArrayHeapTests
    {
        private static ArrayHeap<int, string> CreateHeap(int arity)
        {
            return CreateHeap(arity, new List<KeyValuePair<int, string>>());
        }

        private static ArrayHeap<int, string> CreateHeap(int arity, IEnumerable<KeyValuePair<int, string>> collection)
        {
            return new ArrayHeap<int, string>(Comparer<int>.Default, arity, new List<KeyValuePair<int, string>>(collection));
        }
        
        public static IEnumerable<int> GetAritiesToTest()
        {
            foreach (var arity in Enumerable.Range(1, 20))
                yield return arity;

            yield return 50;
            yield return 100;
            yield return 500;
            yield return 1000;
            yield return 10000;
            yield return 50000;
        }

        [Test]
        public void Should_not_allow_creating_heap_with_null_collection()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var heap = new ArrayHeap<int, int>(Comparer<int>.Default, 2, null);
            });
        }

        [Test]
        public void Should_not_allow_creating_heap_with_arity_less_than_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                // ReSharper disable once UnusedVariable
                var heap = new ArrayHeap<int, string>(Comparer<int>.Default, 0);
            });

            Assert.DoesNotThrow(() =>
            {
                // ReSharper disable once UnusedVariable
                var heap = new ArrayHeap<int, string>(Comparer<int>.Default, 1);
            });
        }
        
        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Should_not_allow_extracting_or_removing_top_from_empty_heap(int arity)
        {
            // Arrange
            var heap = CreateHeap(arity);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
        }

        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Should_heapify_collection_correctly(int arity)
        {
            // Arrange
            var items = Utils.GenerateList(2000, -200, 200)
                .Select(x => new KeyValuePair<int, string>(x, Guid.NewGuid().ToString()))
                .ToArray();

            var heap = CreateHeap(arity, items);

            // Act & Assert
            foreach (var item in items.OrderBy(x => x.Key))
            {
                Assert.AreEqual(false, heap.IsEmpty);
                Assert.AreEqual(item.Key, heap.Peek().Key);
                Assert.AreEqual(item.Key, heap.Pop().Key);
            }

            Assert.AreEqual(true, heap.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => heap.Peek());
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
        }

        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Elements_should_be_added_and_popped_correctly(int arity)
        {
            // Arrange
            var items = Utils.GenerateList(2000, -200, 200);
            var heap = CreateHeap(arity);
            var count = 0;

            // Act
            foreach (var item in items)
            {
                Assert.AreEqual(count, heap.Count);
                heap.Add(item, Guid.NewGuid().ToString());
                Assert.AreEqual(++count, heap.Count);
            }

            // Assert
            foreach (var item in items.OrderBy(x => x))
            {
                Assert.AreEqual(false, heap.IsEmpty);
                Assert.AreEqual(item, heap.Peek().Key);

                Assert.AreEqual(count, heap.Count);
                Assert.AreEqual(item, heap.Pop().Key);
                Assert.AreEqual(--count, heap.Count);
            }

            Assert.AreEqual(true, heap.IsEmpty);
        }
    }
}
