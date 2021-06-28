using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable
    {
        private TreeNode<T> root;

        public void Add(T key)
        {
            if (root == null)
                root = new TreeNode<T>(key);
            else
            {
                var currentNode = root;
                for (var diff = currentNode.Value.CompareTo(key); diff != 0; diff = currentNode.Value.CompareTo(key))
                {
                    currentNode.Weight++;
                    if (diff > 0)
                    {
                        if (currentNode.Left == null)
                            currentNode.Left = new TreeNode<T>(key);
                        currentNode = currentNode.Left;
                    }
                    else
                    {
                        if (currentNode.Right == null)
                            currentNode.Right = new TreeNode<T>(key);
                        currentNode = currentNode.Right;
                    }
                }
            }
        }

        public bool Contains(T key)
        {
            var currentNode = root;
            while (currentNode != null)
            {
                var diff = currentNode.Value.CompareTo(key);
                if (diff == 0)
                    return true;
                currentNode = diff < 0 ? currentNode.Right : currentNode.Left;
            }

            return false;
        }

        public T this[int index]
        {
            get
            {
                if (root == null || index < 0 || index >= root.Weight)
                    throw new IndexOutOfRangeException();
                var (currentNode, parentWeight) = (root, 0);
                while (true)
                {
                    var currentNodeIndex = parentWeight + (currentNode?.Left?.Weight ?? 0);
                    if (index == currentNodeIndex)
                        return currentNode.Value;
                    if (index < currentNodeIndex)
                        currentNode = currentNode.Left;
                    else
                        (currentNode, parentWeight) = (currentNode.Right, currentNodeIndex + 1);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() =>
            root?.GetEnumerator().GetEnumerator() ?? Enumerable.Empty<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class TreeNode<T> where T : IComparable
    {
        public T Value { get; }

        public int Weight { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }

        public TreeNode(T value)
        {
            Value = value;
            Weight = 1;
        }

        public IEnumerable<T> GetEnumerator()
        {
            if (Left != null)
                foreach (var value in Left.GetEnumerator())
                    yield return value;

            yield return Value;

            if (Right != null)
                foreach (var value in Right.GetEnumerator())
                    yield return value;
        }
    }
}