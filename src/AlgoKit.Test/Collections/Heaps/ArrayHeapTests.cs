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
        private static ArrayHeap<int> CreateHeap(int arity)
        {
            return CreateHeap(arity, new List<int>());
        }

        private static ArrayHeap<int> CreateHeap(int arity, IList<int> collection)
        {
            return new ArrayHeap<int>(Comparer<int>.Default, arity, new List<int>(collection));
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
                var heap = new ArrayHeap<int>(Comparer<int>.Default, 2, null);
            });
        }

        [Test]
        public void Should_not_allow_creating_heap_with_arity_less_than_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var heap = new ArrayHeap<int>(Comparer<int>.Default, 0);
            });

            Assert.DoesNotThrow(() =>
            {
                var heap = new ArrayHeap<int>(Comparer<int>.Default, 1);
            });
        }
        
        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Should_not_allow_extracting_or_removing_top_from_empty_heap(int arity)
        {
            // Arrange
            var heap = CreateHeap(arity);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.Top());
            Assert.Throws<InvalidOperationException>(() => heap.Pop());
        }

        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Should_heapify_collection_correctly(int arity)
        {
            // Arrange
            var items = Utils.GenerateList(2000, -200, 200);
            var heap = CreateHeap(arity, items);

            // Act & Assert
            foreach (var item in items.OrderBy(x => x))
            {
                Assert.AreEqual(false, heap.IsEmpty);
                Assert.AreEqual(item, heap.Top());
                Assert.AreEqual(item, heap.Pop());
            }

            Assert.AreEqual(true, heap.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => heap.Top());
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
    }
}
