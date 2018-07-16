using System.Collections.Generic;

namespace MazeLibrary
{
    public sealed class Node<T>
    {
        public T Point { get; private set; }
        public List<Node<T>> Leafs { get; set; }
        public Node<T> Root { get; set; }

        public Node(T point)
        {
            Leafs = new List<Node<T>>();

            Point = point;
        }

        public void Add(Node<T> leaf)
        {
            Leafs.Add(leaf);
        }
    }
}
