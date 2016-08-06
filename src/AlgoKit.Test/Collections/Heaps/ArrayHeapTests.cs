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
            return new ArrayHeap<int>(arity, new List<int>(collection), Comparer<int>.Default);
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
                var heap = new ArrayHeap<int>(2, null, Comparer<int>.Default);
            });
        }

        [Test]
        public void Should_not_allow_creating_heap_with_arity_less_than_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var heap = new ArrayHeap<int>(0, Comparer<int>.Default);
            });

            Assert.DoesNotThrow(() =>
            {
                var heap = new ArrayHeap<int>(1, Comparer<int>.Default);
            });
        }

        [Test]
        public void Should_not_allow_creating_heap_with_null_comparer()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var heap = new ArrayHeap<int>(2, null);
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
            Assert.Throws<InvalidOperationException>(() => heap.RemoveTop());
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
            Assert.Throws<InvalidOperationException>(() => heap.RemoveTop());
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

        [TestCaseSource(nameof(GetAritiesToTest))]
        public void Elements_should_be_replaced_correctly(int arity)
        {
            // Arrange
            var items = Utils.GenerateList(5000, -2500, 2500)
                .Distinct()
                .Select(x => new PairWithPriority<int, string>(x, Guid.NewGuid().ToString()))
                .ToList();

            var dictionary = items.ToDictionary(x => x.Key, x => x.Value);

            var heap = new ArrayHeap<PairWithPriority<int, string>>(arity, items, Comparer<PairWithPriority<int, string>>.Default);

            Func<int, int> keyAt = i => heap.ElementAt(i).Key;
            Func<int, string> valueAt = i => heap.ElementAt(i).Value;

            // Act & Assert - scenario I
            var ten = valueAt(10);
            dictionary.Remove(keyAt(10));
            dictionary.Add(-3000, ten);

            Assert.AreNotEqual(valueAt(0), ten);
            heap.Replace(10, new PairWithPriority<int, string>(-3000, ten));
            Assert.AreEqual(valueAt(0), ten);

            // Act & Assert - scenario II
            var rnd = new Random();
            for (var i = 1; i < 500; ++i)
            {
                var index = rnd.Next(0, heap.Count);
                var sign = rnd.NextDouble() > 0.5 ? 1 : -1;
                var newKey = sign * (3000 + i);
                var item = heap.ElementAt(index).Value;

                dictionary.Remove(keyAt(index));
                dictionary.Add(newKey, item);
                heap.Replace(index, new PairWithPriority<int, string>(newKey, item));
            }

            var ordered = dictionary.Keys.OrderBy(x => x);
            foreach (var key in ordered)
            {
                var min = heap.Pop();
                Assert.AreEqual(key, min.Key);
                Assert.AreEqual(dictionary[key], min.Value);
            }
        }
    }
}
