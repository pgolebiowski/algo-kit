using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Internal representation of the binomial tree collection.
    /// </summary>
    internal class BinomialTreeCollection<T> where T : IComparable<T>
    {
        public int Count { get; private set; }
        private LinkedList<BinomialTree<T>> trees;

        public BinomialTree<T> TreeWithTopElement => this.FindTreeWithTopElement().Value;

        public BinomialTreeCollection()
        {
            this.trees = new LinkedList<BinomialTree<T>>();
            this.Count = 0;
        }

        public BinomialTree<T> Pop()
        {
            var treeNodeWithTopElement = this.FindTreeWithTopElement();
            var tree = treeNodeWithTopElement.Value;

            this.trees.Remove(treeNodeWithTopElement);
            this.Count -= tree.Count;

            return tree;
        }

        /// <complexity>
        /// Finds the minimum among the roots of the binomial trees. This is done in O(log n) time, 
        /// as there are just O(log n) trees (and hence roots to examine).
        /// </complexity>
        private LinkedListNode<BinomialTree<T>> FindTreeWithTopElement()
        {
            if (this.Count == 0)
               return null;

            var top = this.trees.First;
            var topValue = top.Value.Root.Value;

            for (var node = top.Next; node != null; node = node.Next)
            {
                var rootValue = node.Value.Root.Value;

                if (rootValue.CompareTo(topValue) < 0)
                {
                    top = node;
                    topValue = rootValue;
                }
            }

            return top;
        }

        public void AddLast(BinomialTree<T> tree)
        {
            this.trees.AddLast(tree);
            this.Count += tree.Count;
        }

        public void Merge(BinomialTree<T> tree)
        {
            var collection = new LinkedList<BinomialTree<T>>();
            collection.AddLast(tree);
            this.Count += tree.Count;
            this.Merge(collection);
        }

        public void Merge(BinomialTreeCollection<T> other)
        {
            this.Merge(other.trees);
            this.Count += other.Count;
        }

        private void Merge(LinkedList<BinomialTree<T>> other)
        {
            var result = new LinkedList<BinomialTree<T>>();

            var i = this.trees.First;
            var j = other.First;

            while (i != null && j != null)
            {
                var iOrder = i.Value.Order;
                var jOrder = j.Value.Order;

                BinomialTree<T> tree;

                if (iOrder < jOrder)
                {
                    tree = i.Value;
                    i = i.Next;
                }
                else if (jOrder < iOrder)
                {
                    tree = j.Value;
                    j = j.Next;
                }
                else
                {
                    tree = i.Value.Merge(j.Value);
                    i = i.Next;
                    j = j.Next;
                }

                var last = result.Last?.Value;
                if (last == null || (last.Order < tree.Order))
                {
                    result.AddLast(tree);
                    continue;
                }

                // last.Order == tree.Order
                result.RemoveLast();
                result.AddLast(last.Merge(tree));
            }

            if (i == null && j == null)
            {
                this.trees = result;
                return;
            }

            var k = i ?? j;
            var kCol = i == null ? other : this.trees;
            while (k != null)
            {
                var last = result.Last?.Value;

                if (last == null || (last.Order < k.Value.Order))
                {
                    // we can add all the remaining trees - there will be no overflow
                    var next = k.Next;
                    do
                    {
                        kCol.Remove(k);
                        result.AddLast(k);
                        k = next;
                        next = next?.Next;

                    } while (k != null);
                    break;
                }

                // last.Order == tree.Order
                result.RemoveLast();
                result.AddLast(last.Merge(k.Value));
                k = k.Next;
            }

            this.trees = result;
        }
    }
}