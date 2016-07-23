using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Collections.Heaps;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace AlgoKit.Test.Collections.Heaps
{
    [TestFixture]
    public class MinBinaryHeapTests
    {
        [Test]
        public void Should_not_allow_extracting_or_deleting_minimum_from_empty_heap()
        {
            // Arrange
            var heap = new MinBinaryHeap<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.ExtractMin(), "The heap is empty.");
            Assert.Throws<InvalidOperationException>(() => heap.DeleteMin(), "The heap is empty.");
        }

        [Test]
        public void Should_allow_extracting_minimum_as_long_as_the_heap_is_not_empty()
        {
            // Arrange
            var heap = new MinBinaryHeap<int>(new List<int> {3, 1, 2});

            // Act & Assert
            Assert.AreEqual(1, heap.ExtractMin());
            Assert.AreEqual(2, heap.ExtractMin());
            Assert.AreEqual(3, heap.ExtractMin());
            Assert.Throws<InvalidOperationException>(() => heap.ExtractMin(), "The heap is empty.");
            Assert.Throws<InvalidOperationException>(() => heap.DeleteMin(), "The heap is empty.");
        }

        [Test]
        public void Should_heapify_a_collection_correctly()
        {
            // Arrange
            var fixture = new Fixture {RepeatCount = 10000};
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(-1000, 1000));
            var collection = fixture.Create<List<int>>();

            var ordered = collection.OrderBy(x => x);
            var heap = new MinBinaryHeap<int>(collection);

            // Act & Assert
            foreach (var element in ordered)
                Assert.AreEqual(element, heap.ExtractMin());

            Assert.Throws<InvalidOperationException>(() => heap.ExtractMin(), "The heap is empty.");
        }

        [Test]
        public void Elements_should_be_inserted_correctly()
        {
            // Arrange
            var fixture = new Fixture { RepeatCount = 10000 };
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(-1000, 1000));
            var collection = fixture.Create<List<int>>();

            var ordered = collection.OrderBy(x => x);
            var heap = new MinBinaryHeap<int>();

            // Act
            foreach (var element in collection)
                heap.Insert(element);

            // Assert
            foreach (var element in ordered)
                Assert.AreEqual(element, heap.ExtractMin());

            Assert.Throws<InvalidOperationException>(() => heap.ExtractMin(), "The heap is empty.");
        }

        [Test]
        public void Elements_should_be_updated_properly()
        {
            // Arrange
            var fixture = new Fixture { RepeatCount = 5000 };
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(-2000, 2000));
            var collection = fixture
                .Create<List<int>>()
                .Distinct()
                .Select(x => new PairWithPriority<int, string>(x, Guid.NewGuid().ToString()))
                .ToList();

            var dictionary = collection.ToDictionary(x => x.Key, x => x.Value);

            var heap = new MinBinaryHeap<PairWithPriority<int, string>>(collection);

            Func<int, int> keyAt = i => heap.ElementAt(i).Key;
            Func<int, string> valueAt = i => heap.ElementAt(i).Value;

            // Act & Assert - scenario I
            var ten = valueAt(10);

            dictionary.Remove(keyAt(10));
            dictionary.Add(-3000, ten);

            Assert.AreNotEqual(valueAt(0), ten);
            heap.Update(10, new PairWithPriority<int, string>(-3000, ten));
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
                heap.Update(index, new PairWithPriority<int, string>(newKey, item));
            }

            var ordered = dictionary.Keys.OrderBy(x => x);
            foreach (var key in ordered)
            {
                var min = heap.ExtractMin();
                Assert.AreEqual(key, min.Key);
                Assert.AreEqual(dictionary[key], min.Value);
            }
        }
    }
}
