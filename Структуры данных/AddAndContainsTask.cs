using System;

namespace BinaryTrees
{
    public class BinaryTree<T> where T : IComparable
    {
        private TreeNode<T> root;

        public void Add(T key)
        {
            if (root == null)
                root = new TreeNode<T>(key);
            else
                Add(key, root);
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
		
		private void Add(T key, TreeNode<T> rootNode, bool isAdded = false)
        {
            while (!isAdded)
                if (rootNode.Value.CompareTo(key) <= 0)
                    if (rootNode.Right != null)
                        rootNode = rootNode.Right;
                    else
                        (rootNode.Right, isAdded) = (new TreeNode<T>(key), true);
                else if (rootNode.Left != null)
                    rootNode = rootNode.Left;
                else
                    (rootNode.Left, isAdded) = (new TreeNode<T>(key), true);
        }
    }

    public class TreeNode<T> where T : IComparable
    {
        public T Value { get; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }
    }
}