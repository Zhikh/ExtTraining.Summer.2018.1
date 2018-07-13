using System;
using System.Collections.Generic;

namespace MazeLibrary
{
    public class MazeSolver
    {
        private readonly int _wall = -1;
        private readonly int _space = 0;

        private int _startX;
        private int _startY;

        private int[,] temp;

        private int _step = 1;

        enum Direction { Left, Right, Down, Up, Start };
        List<List<(int X, int Y)>> _ways;

        #region Public
        public MazeSolver(int[,] mazeModel, int startX, int startY)
        {
            if (mazeModel == null)
            {
                throw new ArgumentNullException("Object can't be null!");
            }

            if (startX < 0 || startY < 0)
            {
                throw new ArgumentException("Coordinates can't be less than 0!");
            }

            if (mazeModel[startX, startY] == _wall)
            {
                throw new ArgumentException("It's not the enter!");
            }

            _startX = startX;
            _startY = startY;

            temp = new int[mazeModel.GetLength(0), mazeModel.GetLength(1)];
            Copy(temp, mazeModel);
            _ways = new List<List<(int X, int Y)>>();
            temp[startX, startY] = _step;
        }

        public int[,] MazeWithPass()
        {
            return temp;
        }

        public void PassMaze()
        {
            var list = new List<(int X, int Y)>();
            list.Add((_startX, _startY));
            var treeOfWays = new Node<(int X, int Y)>((_startX, _startY));

            MoveNext(_startX, _startY, treeOfWays);
            GoIntoDeep(treeOfWays, list);
            SetPas(FindMinIndex());
        }
        #endregion

        #region Private
        private int FindMinIndex()
        {
            int min = _ways[0].Count;
            int result = 0;
            int i = 0;
            foreach (var way in _ways)
            {
                if (way.Count < min)
                {
                    min = way.Count;
                    result = i;
                }
                i++;
            }

            return result;
        }

        private void SetPas(int number)
        {
            int step = _ways[number].Count;

            foreach (var i in _ways[number])
            {
                temp[i.X, i.Y] = step--;
            }
        }

        private void GoIntoDeep(Node<(int X, int Y)> node, List<(int X, int Y)> way)
        {
            foreach (var leaf in node.Leafs)
            {
                if (!IsBlockWay(leaf.Point))
                {
                    way.Add(leaf.Point);
                    if (leaf.Leafs.Count != 0)
                    {
                        GoIntoDeep(leaf, way);
                    }
                    else
                    {
                        List<(int, int)> wayToSave = new List<(int, int)>();

                        foreach (var element in way)
                        {
                            wayToSave.Add(element);
                        }

                        _ways.Add(wayToSave);
                    }
                }
            }
        }
        private void Copy(int[,] left, int[,] right)
        {
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    left[i, j] = right[i, j];
                }
            }
        }

        private void MoveNext(int x, int y, Node<(int X, int Y)> node, Direction direction = Direction.Start)
        {
            if (direction != Direction.Right && TryMoveLeft(x, y))
            {
                var leaf = new Node<(int X, int Y)>((x, --y));
                node.Add(leaf);
                MoveNext(x, y, leaf, Direction.Left);
            }

            if (direction != Direction.Left && TryMoveRigth(x, y))
            {
                var leaf = new Node<(int X, int Y)>((x, ++y));
                node.Add(leaf);
                MoveNext(x, y, leaf, Direction.Right);
            }

            if (direction != Direction.Up && TryMoveDown(x, y))
            {
                var leaf = new Node<(int X, int Y)>((++x, y));
                node.Add(leaf);
                MoveNext(x, y, leaf, Direction.Down);
            }

            if (direction != Direction.Down && TryMoveUp(x, y))
            {
                var leaf = new Node<(int X, int Y)>((--x, y));
                node.Add(leaf);
                MoveNext(x, y, leaf, Direction.Up);
            }
        }

        private bool IsBlockWay((int X, int Y) Point)
        {
            int wallsCount = 0;

            if (Point.X > 0 && IsWall(Point.X - 1, Point.Y))
            {
                wallsCount++;
            }

            if (Point.Y > 0 && IsWall(Point.X, Point.Y - 1))
            {
                wallsCount++;
            }

            if (Point.X < temp.GetLength(0) - 1 && IsWall(Point.X + 1, Point.Y))
            {
                wallsCount++;
            }

            if (Point.Y < temp.GetLength(1) - 1 && IsWall(Point.X, Point.Y + 1))
            {
                wallsCount++;
            }

            if (wallsCount == 3)
                return true;

            return false;
        }

        private bool IsWall(int x, int y)
        {
            if (temp[x, y] == _wall)
            {
                return true;
            }

            return false;
        }

        private bool TryMoveLeft(int i, int j)
        {
            if (j > 0)
            {
                if (temp[i, --j] == _space)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryMoveRigth(int i, int j)
        {
            if (j < temp.GetLength(1) - 1)
            {
                if (temp[i, ++j] == _space)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TryMoveDown(int i, int j)
        {
            if (i < (temp.GetLength(0) - 1))
            {
                if (temp[++i, j] == _space)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryMoveUp(int i, int j)
        {
            if (i > 0)
            {
                if (temp[--i, j] == _space)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
