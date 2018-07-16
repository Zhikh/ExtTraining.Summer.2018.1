using System;
using System.Collections.Generic;
namespace MazeLibrary
{
    public static class Maze
    {
        private static readonly int _space = 0;
        private static readonly int _wall = -1;
        private static readonly int _visited = -2;
        private static (int X, int Y) _start;
        private static int _n, _m;

        #region Public API
        public static  int PassMaze(int[,] array, int x, int y)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Object can't be null!");
            }

            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Coordinates can't be less than 0!");
            }

            if (array[x, y] == _wall)
            {
                throw new ArgumentException("It's not the enter!");
            }

            _start = (x, y);

            _n = array.GetLength(0) - 1;
            _m = array.GetLength(1) - 1;

            var treeOfWays = new Node<(int X, int Y)>(_start);
            if (!MoveNext(array.Copy(), treeOfWays))
            {
                return -1;
            }

            if (treeOfWays.Leafs.Count == 0 )
            {
                return -1;
            }

            SetPass(treeOfWays, array);

            return 0;
        }
        #endregion

        #region Private methods
        private static void SetPass(Node<(int X, int Y)> node, int[,] array)
        {
            var list = new List<(int X, int Y)>();
            list.Add(_start);

            var ways = new List<List<(int X, int Y)>>();
            GoIntoDeep(node, list, ways);
            var way = FindMinWay(ways);

            int step = way.Count;
            foreach (var point in way)
            {
                array[point.X, point.Y] = step--;
            }
        }

        private static List<(int X, int Y)> FindMinWay(List<List<(int X, int Y)>> ways)
        {
            int i = 0;
            List<(int X, int Y)> minWay = ways[i++];
            for (; i < ways.Count; i++)
            {
                if (ways[i].Count < minWay.Count)
                {
                    minWay = ways[i];
                }
            }
            return minWay;
        }

        private static void GoIntoDeep(Node<(int X, int Y)> node, List<(int X, int Y)> way, List<List<(int X, int Y)>> ways)
        {
            foreach (var leaf in node.Leafs)
            {
                way.Add(leaf.Point);
                if (leaf.Leafs.Count != 0)
                {
                    GoIntoDeep(leaf, way, ways);
                }
                else
                {
                    List<(int, int)> wayToSave = new List<(int, int)>();

                    foreach (var element in way)
                    {
                        wayToSave.Add(element);
                    }

                    ways.Add(wayToSave);
                }
            }
        }

        private static bool MoveNext(int[,] maze, Node<(int X, int Y)> node)
        {
            int x = node.Point.X, 
                y = node.Point.Y;
            int parentX = node.Root != null ? node.Root.Point.X : -1,
                parentY = node.Root != null ? node.Root.Point.Y : -1;

            bool right = y < _m ? (maze[x, y + 1] == _space) && !(parentX == x && parentY == y + 1) : false;
            bool left = y > 0 ? maze[x, y - 1] == _space && !(parentX == x && parentY == y - 1) : false;
            bool up = x > 0 ? maze[x - 1, y] == _space && !(parentX == x - 1 && parentY == y) : false;
            bool down = x < _n ? maze[x + 1, y] == _space && !(parentX == x + 1 && parentY == y) : false;

            int directions = 0;

            IncValue(right, ref directions);
            IncValue(left, ref directions);
            IncValue(up, ref directions);
            IncValue(down, ref directions);

            if (directions == 0)
            {
                if (IsBlock(maze, node))
                {
                    maze[x, y] = _wall;
                    return ComeBack(maze, node);
                }

                if (IsEnd(node))
                {
                    return true;
                }

                return false;
            }

            if (directions == 1)
            {
                maze[x, y] = _visited;
            }
            TryMove(maze, x - 1, y, up, node);
            TryMove(maze, x + 1, y, down, node);

            TryMove(maze, x, y + 1, right, node);
            TryMove(maze, x, y - 1, left, node);

            return true;
        }

        private static bool ComeBack(int[,] maze, Node<(int X, int Y)> node)
        {
            Node<(int X, int Y)> root = node.Root;

            if (root.Point.X == _start.X && root.Point.Y == _start.Y)
            {
                return false;
            }

            root.Leafs.Remove(node);

            return MoveNext(maze, root);
        }

        private static bool IsEnd(Node<(int X, int Y)> node)
        {
            int x = node.Point.X,
                y = node.Point.Y;

            return y == _m || y == 0 || x == _n || x == 0;
        }

        private static bool IsBlock(int[,] maze, Node<(int X, int Y)> node)
        {
            int x = node.Point.X,
                y = node.Point.Y;

            bool right = y < _m ? maze[x, y + 1] == _wall : false;
            bool left = y > 0 ? maze[x, y - 1] == _wall : false;
            bool up = x > 0 ? maze[x - 1, y] == _wall : false;
            bool down = x < _n ? maze[x + 1, y] == _wall : false;

            int walls = 0;

            IncValue(right, ref walls);
            IncValue(left, ref walls);
            IncValue(up, ref walls);
            IncValue(down, ref walls);

            return walls == 3;
        }

        private static void TryMove(int[,] maze, int x, int y, bool flag, Node<(int X, int Y)> node)
        {
            if (flag)
            {
                var leaf = new Node<(int X, int Y)>((x, y));
                leaf.Root = node;
                node.Add(leaf);
                MoveNext(maze, leaf);
            }
        }

        private static void IncValue(bool value, ref int directions)
        {
            if (value)
            {
                directions++;
            }
        }
        #endregion
    }
}
