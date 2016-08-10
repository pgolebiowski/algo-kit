using System;
using System.Collections.Generic;

namespace AlgoKit.Collections.Heaps
{
    /// <summary>
    /// Represents a forest of uniquely-sized, heap-ordered binomial trees
    /// in a child-sibling form (binary tree).
    /// </summary>
    public class BinomialHeap<T> : BaseHeap<T, BinomialHeapNode<T>, BinomialHeap<T>>
    {
        private readonly BinomialHeapNode<T>[] roots;
        private BinomialHeapNode<T> top;
        private IComparer<T> fakeComparer = Comparer<T>.Create((x, y) => -1);
        private int count;

        /// <summary>
        /// Creates an empty binomial heap.
        /// </summary>
        /// <param name="comparer">
        /// The comparer used to determine whether one object should be extracted
        /// from the heap earlier than the other one.
        /// </param>
        public BinomialHeap(IComparer<T> comparer)
        {
            this.Comparer = comparer;
            this.roots = new BinomialHeapNode<T>[32];
        }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public override int Count => this.count;

        /// <summary>
        /// Gets the top element of the heap.
        /// </summary>
        public override T Top()
        {
            if (this.IsEmpty)
                throw new InvalidOperationException("The heap is empty.");

            return this.top.Value;
        }

        /// <summary>
        /// Returns the top element after removing it from the heap.
        /// </summary>
        public override T Pop()
        {
            // Get the root with the top value and remove it from the collection
            // of binomial trees.

            var result = this.Top();
            var toRemove = this.top;

            this.top = null;
            this.roots[toRemove.Rank] = null;

            // Now we should break the tree represented by the node 'toRemove' apart
            // and insert back all its subtrees to our list of binomial trees.

            this.BreakTreeAndInsertChildren(toRemove);

            // The old top node is ready to be garbage collected (unless the user
            // owns a reference to it). Adjust the number of elements and update the top.

            --this.count;
            this.UpdateTop();

            return result;
        }

        /// <summary>
        /// Adds an object to the heap.
        /// </summary>
        public override BinomialHeapNode<T> Add(T item)
        {
            // Create a one-node tree for the specified item and perform
            // the usual merging of heaps.

            var tree = new BinomialHeapNode<T>(item);

            this.MakeRoot(tree);
            ++this.count;

            return tree;
        }

        /// <summary>
        /// Removes an arbitrary node from the heap.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        public override T Remove(BinomialHeapNode<T> node)
        {
            // To delete a node from the heap we will move it up to the top and
            // then simply pop it. Instead of assigning some 'hacky' values like 
            // negative infinity, we will temporarily use a fake comparer that always
            // says the first parameter is smaller.

            var result = node.Value;

            this.SwapRealAndFakeComparer();
            this.MoveUp(node);
            this.SwapRealAndFakeComparer();

            this.Pop();

            return result;
        }

        /// <summary>
        /// Updates the value contained in the specified node.
        /// </summary>
        /// <param name="node">The node to update.</param>
        /// <param name="value">The new value for the node.</param>
        public override void Update(BinomialHeapNode<T> node, T value)
        {
            var relation = this.Comparer.Compare(value, node.Value);
            node.Value = value;

            // If the new value is considered equal to the previous value, there is no need
            // to fix the heap property, because it is already preserved.

            if (relation < 0)
                this.MoveUp(node);
            else if (relation > 0)
            {
                // If one of the roots is moved down (or it is not moved, but its value is 
                // modified) then there is a chance that the top should be updated as well.
                var isRootBeingUpdated = node.Parent == null;

                this.MoveDown(node);

                if (isRootBeingUpdated)
                    this.UpdateTop();
            }
        }

        /// <summary>
        /// Merges this heap with another heap, destroying it.
        /// </summary>
        /// <param name="other">The heap to be merged with this heap.</param>
        public override void Meld(BinomialHeap<T> other)
        {
            this.count += other.count;
            foreach (var root in other.roots)
                this.MakeRoot(root);
        }

        /// <summary>
        /// Swaps the comparer passed by the user in the constructor with a fake
        /// comparer. This method is used twice during node removal.
        /// </summary>
        private void SwapRealAndFakeComparer()
        {
            var tmp = this.Comparer;
            this.Comparer = this.fakeComparer;
            this.fakeComparer = tmp;
        }

        /// <summary>
        /// Moves the node up in the tree to restore heap order.
        /// </summary>
        /// <param name="node">The node to be moved up.</param>
        private void MoveUp(BinomialHeapNode<T> node)
        {
            // Exchange the element with its parent, and possibly also with its
            // grandparent, and so on, until the heap property is no longer violated.
            // Each binomial tree has height at most log n, so this takes O(log n) time.

            while (node.Parent != null)
            {
                var parent = node.Parent;
                var current = node;

                // Remember we want to go up in a binomial tree having a binary tree.
                // For that reason, we need to find a parent in a binary tree. Thus,
                // go up until you reach the leftmost sibling. From there, go up once more.
                // You'll reach the parent in the binomial tree.

                while (parent.Right == current)
                {
                    current = current.Parent;
                    parent = current.Parent;
                }

                if (this.Comparer.Compare(node.Value, parent.Value) < 0)
                    this.SwapWithParent(node, parent);
                else
                    break;
            }

            // If we got to the very top of our tree, there is a chance that the global
            // top (for the collection of binomial trees) should be updated as well.

            if (this.Comparer.Compare(node.Value, this.top.Value) < 0)
                this.top = node;
        }

        /// <summary>
        /// Moves the node down in the tree to restore heap order.
        /// </summary>
        /// <param name="node">The node to be moved down.</param>
        private void MoveDown(BinomialHeapNode<T> node)
        {
            // We want to move one node down in a binomial tree. However,
            // what we have is a binary tree. We need to go to the first child
            // and then iterate all the way right (to check all siblings). Our goal
            // is to find the child that should be extracted first from the heap.
            // Repeat the process until the heap property is restored.

            // Check if the current node should really be extracted first, or maybe
            // one of its children should be extracted earlier.

            var topChild = node.Left;

            // In case the current node has no children, there is nothing more to do.
            // It cannot be moved down any further.
            if (topChild == null)
                return;

            // There are some children, so browse them and find the top one.
            for (var child = topChild.Right; child != null; child = child.Right)
                if (this.Comparer.Compare(child.Value, topChild.Value) < 0)
                    topChild = child;

            // In case no child needs to be extracted earlier than the current node,
            // there is nothing more to do - the right spot was found.
            if (this.Comparer.Compare(node.Value, topChild.Value) <= 0)
                return;

            // Move the top child up by one node and now investigate the
            // node that was considered to be the top child (recursive).
            this.SwapWithParent(topChild, node);

            this.MoveDown(node);
        }

        /// <summary>
        /// Breaks a tree represented by the specified node apart and inserts all its
        /// subtrees to the list of binomial trees.
        /// </summary>
        /// <param name="node">The root of the tree to break.</param>
        private void BreakTreeAndInsertChildren(BinomialHeapNode<T> node)
        {
            var current = node.Left;
            while (current != null)
            {
                var next = current.Right;
                this.MakeRoot(current);
                current = next;
            }
        }

        /// <summary>
        /// Makes an arbitrary node a root.
        /// </summary>
        /// <param name="node">Node to be made a root.</param>
        private void MakeRoot(BinomialHeapNode<T> node)
        {
            // Roots have no parents and siblings, so adjust the pointers accordingly.

            node.Parent = null;
            node.Right = null;

            // Since we insert a new root to out collection of binomial trees,
            // there is a chance that the global top should be updated as well.

            if (this.top == null || this.Comparer.Compare(node.Value, this.top.Value) < 0)
                this.top = node;

            // Now, as long as the appropriate slot for our tree is occupied by another
            // tree with the same rank, join the two and continue the process.

            while (this.TryInsert(node, out node) == false)
            {
            }
        }

        /// <summary>
        /// Tries to insert a tree into a collection of binomial trees. If the operation
        /// fails, it returns a merged tree.
        /// </summary>
        /// <param name="node">The root of the tree to insert.</param>
        /// <param name="merged">The result of merging trees if the appropriate slot is occupied.</param>
        private bool TryInsert(BinomialHeapNode<T> node, out BinomialHeapNode<T> merged)
        {
            // If the appropriate slot for our tree is empty, simply insert the root
            // and return. However, if there is already a tree with the same rank,
            // join the two and pass it as the out parameter.

            var rank = node.Rank;
            var root = this.roots[rank];

            if (root == null)
            {
                this.roots[rank] = node;
                merged = null;
                return true;
            }

            this.roots[rank] = null;
            merged = this.Merge(root, node);
            return false;
        }

        /// <summary>
        /// Swaps a node with its parent.
        /// </summary>
        private void SwapWithParent(BinomialHeapNode<T> node, BinomialHeapNode<T> parent)
        {
            // Remember that binomial trees are represented here as binary trees.
            // This makes thinking about swapping much easier.

            var np = node.Parent;
            var nl = node.Left;
            var nr = node.Right;

            var pp = parent.Parent;
            var pl = parent.Left;
            var pr = parent.Right;

            // Let's start from adjusting the ranks. Basic swapping.

            var tmp = node.Rank;
            node.Rank = parent.Rank;
            parent.Rank = tmp;

            // For the node and its parent we will now adjust their
            // left and right pointers.

            parent.Left = nl;
            if (nl != null)
                nl.Parent = parent;

            parent.Right = nr;
            if (nr != null)
                nr.Parent = parent;

            node.Right = pr;
            if (pr != null)
                pr.Parent = node;

            // To adjust the left pointer of the node we need to consider two cases,
            // depending on whether our 'node' in the binary tree is the left child of
            // the 'parent' (equivalently, it is also the first child of the 'parent'
            // in the corresponding binomial tree then).

            if (node == pl)
            {
                // 'node' is indeed the left child of the 'parent', so swapping is simple.
                parent.Parent = node;
                node.Left = parent;
            }
            else
            {
                // 'node' is not the left child of the 'parent', so we also need to adjust
                // 'pl' and 'np', which are some other nodes.
                node.Left = pl;
                if (pl != null)
                    pl.Parent = node;

                parent.Parent = np;
                np.Right = parent;
            }

            // Now we're left with adjusting the grandparent of our node.

            node.Parent = pp;
            if (pp == null)
            {
                // Our node's parent was a root, so right now the node is a new root.
                // We need to update the array of roots then. The top *could* be updated
                // here, but this is done elsewhere as an optimization.
                this.roots[node.Rank] = node;
            }
            else
            {
                // Our node is not a new root.
                if (pp.Left == parent)
                    pp.Left = node;
                else
                    pp.Right = node;
            }
        }

        /// <summary>
        /// Iterates through the collection of binomial trees and updates the top node.
        /// </summary>
        private void UpdateTop()
        {
            for (var i = 0; i < this.roots.Length; ++i)
            {
                var root = this.roots[i];

                if (root == null)
                    continue;

                if (this.top == null)
                {
                    this.top = root;
                    continue;
                }

                if (this.Comparer.Compare(root.Value, this.top.Value) < 0)
                    this.top = root;
            }
        }

        /// <summary>
        /// Merges two trees of equal rank and returns the root of the resulting heap.
        /// </summary>
        private BinomialHeapNode<T> Merge(BinomialHeapNode<T> a, BinomialHeapNode<T> b)
        {
            // If both binomial heaps are non-empty, the merge function returns
            // a new heap where the smallest root of the two heaps is the root of 
            // the new combined heap and adds the other heap to the list of its children.

            BinomialHeapNode<T> parent, child;

            if (this.Comparer.Compare(a.Value, b.Value) < 0)
            {
                parent = a;
                child = b;
            }
            else
            {
                parent = b;
                child = a;
            }

            // The smallest root will be the new leftmost child of the largest root.
            // Thus, make a parent-child relation.

            child.Right = parent.Left;
            if (parent.Left != null)
                parent.Left.Parent = child;

            child.Parent = parent;
            parent.Left = child;

            ++parent.Rank;

            // Adjust the top if we improved it.

            if (this.top == child)
                this.top = parent;

            return parent;
        }
    }
}
