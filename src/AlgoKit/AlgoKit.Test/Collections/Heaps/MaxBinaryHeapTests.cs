using System;
using System.Collections.Generic;
using System.Linq;
using AlgoKit.Collections.Heaps;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace AlgoKit.Test.Collections.Heaps
{
    [TestFixture]
    public class MaxBinaryHeapTests
    {
        [Test]
        public void Should_not_allow_extracting_or_deleting_maximum_from_empty_heap()
        {
            // Arrange
            var heap = new MaxBinaryHeap<int>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => heap.ExtractMax(), "The heap is empty.");
            Assert.Throws<InvalidOperationException>(() => heap.DeleteMax(), "The heap is empty.");
        }

        [Test]
        public void Should_allow_extracting_maximum_as_long_as_the_heap_is_not_empty()
        {
            // Arrange
            var heap = new MaxBinaryHeap<int>(new List<int> {3, 1, 2});

            // Act & Assert
            Assert.AreEqual(3, heap.ExtractMax());
            Assert.AreEqual(2, heap.ExtractMax());
            Assert.AreEqual(1, heap.ExtractMax());
            Assert.Throws<InvalidOperationException>(() => heap.ExtractMax(), "The heap is empty.");
            Assert.Throws<InvalidOperationException>(() => heap.DeleteMax(), "The heap is empty.");
        }

        [Test]
        public void Should_heapify_a_collection_correctly()
        {
            // Arrange
            var fixture = new Fixture {RepeatCount = 10000};
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(-1000, 1000));
            var collection = fixture.Create<List<int>>();

            var ordered = collection.OrderByDescending(x => x);
            var heap = new MaxBinaryHeap<int>(collection);

            // Act & Assert
            foreach (var element in ordered)
                Assert.AreEqual(element, heap.ExtractMax());

            Assert.Throws<InvalidOperationException>(() => heap.ExtractMax(), "The heap is empty.");
        }

        [Test]
        public void Elements_should_be_inserted_correctly()
        {
            // Arrange
            var fixture = new Fixture { RepeatCount = 10000 };
            fixture.Customizations.Add(new RandomNumericSequenceGenerator(-1000, 1000));
            var collection = fixture.Create<List<int>>();

            var ordered = collection.OrderByDescending(x => x);
            var heap = new MaxBinaryHeap<int>();

            // Act
            foreach (var element in collection)
                heap.Add(element);

            // Assert
            foreach (var element in ordered)
                Assert.AreEqual(element, heap.ExtractMax());

            Assert.Throws<InvalidOperationException>(() => heap.ExtractMax(), "The heap is empty.");
        }
    }
}
