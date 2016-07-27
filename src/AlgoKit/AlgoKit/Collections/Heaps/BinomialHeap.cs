using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a collection of heap-ordered binomial trees.
    /// </summary>
    public class BinomialHeap<T> where T : IComparable<T>
    {
        private readonly BinomialTreeCollection<T> trees;

        /// <summary>
        /// Creates an empty binomial heap.
        /// </summary>
        public BinomialHeap()
        {
            this.trees = new BinomialTreeCollection<T>();
        }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public int Count => this.trees.Count;

        /// <summary>
        /// Returns true if the heap is empty, false otherwise.
        /// </summary>
        public bool IsEmpty => this.Count == 0;

        /// <summary>
        /// Adds an element to the heap.
        /// </summary>
        /// <complexity>
        /// Due to the merge, addition takes O(log n) time. However, across a series of n
        /// consecutive additions, the amortized time is O(1) (i.e. constant).
        /// </complexity>
        public BinomialTreeNode<T> Add(T element)
        {
            // Create a new one-element tree and then merge it with this heap.
            var tree = new BinomialTree<T>(element);
            var reference = tree.Root;
            
            this.trees.Merge(tree);
            return reference;
        }

        /// <summary>
        /// Gets the top element of the heap.
        /// </summary>
        /// <complexity>
        /// By using a pointer to the binomial tree that contains the minimum element,
        /// the time for this operation is O(1). The pointer must be updated when performing
        /// other operations. This can be done without raising the running times.
        /// </complexity>
        public T Top()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            return this.trees.TreeWithTopElement.Root.Value;
        }

        /// <summary>
        /// Returns the top element after removing it from the heap.
        /// </summary>
        public T Pop()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            // Find the top element, remove it from its binomial tree, and obtain a list of its subtrees.
            var treeWithTopElement = this.trees.Pop();
            var subtrees = treeWithTopElement.Root;

            // Transform this list of subtrees into a separate binomial tree collection.
            var collection = new BinomialTreeCollection<T>();
            var order = 0;

            foreach (var subtree in subtrees)
            {
                collection.AddLast(new BinomialTree<T>(subtree, order));
                ++order;
            }

            // Then merge this collection with the original heap.
            this.trees.Merge(collection);

            return treeWithTopElement.Root.Value;
        }

        public BinomialTreeNode<T> Replace(BinomialTreeNode<T> handle, T element)
        {
            // After decreasing the key of an element, it may become smaller than the key of its parent,
            // violating the minimum-heap property.If this is the case, exchange the element with its 
            // parent, and possibly also with its grandparent, and so on, until the minimum - heap property
            // is no longer violated. Each binomial tree has height at most log n, so this takes O(log n) time.

            throw new NotImplementedException();
        }

        public void Remove(BinomialTreeNode<T> handle)
        {
            // To delete an element from the heap, decrease its key to negative infinity (that is, 
            // some value lower than any element in the heap) and then delete the minimum in the heap.

            throw new NotImplementedException();
        }

        /// <summary>
        /// Merges the collection of binomial trees with a collection from another heap.
        /// </summary>
        public void Merge(BinomialHeap<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            
            this.trees.Merge(other.trees);
        }
    }
}
