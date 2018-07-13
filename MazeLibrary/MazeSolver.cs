using System;

namespace MazeLibrary
{
    public class MazeSolver
    {
        private readonly int _wall = -1;
        private readonly int _space = 0;
        private readonly int _block = -2;
        private int _startX;
        private int _startY;
        
        private int[,] temp;

        private int _step = 1;

        enum Direction { Left, Right, Down, Up, Start};

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

            temp[startX, startY] = _step;
        }

        public int[,] MazeWithPass()
        {
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    if (temp[i, j] != _space && temp[i, j] != _wall)
                    {
                        temp[i, j] = _step--;
                    }
                    else
                    {
                        if (temp[i, j] == _block)
                        {
                            temp[i, j] = _space;
                        }
                    }
                }
            }

            return temp;
        }

        public void PassMaze()
        {
            MoveNext(_startX, _startY);
        }
        #endregion

        #region Private
        private bool IsBlockWay(int x, int y)
        {
            int wallsCount = 0;
            
            if (x > 0 && IsWall(x - 1, y))
            {
                wallsCount++;
            }

            if (y > 0 && IsWall(x, y - 1))
            {
                wallsCount++;
            }

            if (x < temp.GetLength(0) && IsWall(x + 1, y))
            {
                wallsCount++;
            }

            if (y < temp.GetLength(1) && IsWall(x, y + 1))
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

        private bool MoveNext(int x, int y, Direction direction = Direction.Start)
        {
            bool result = false;

            if (direction != Direction.Right && TryMoveLeft(x, y))
            {
                //if (!IsBlockWay(x, y))
                //{
                    return MoveNext(x, --y, Direction.Left);
                //}
                //else
                //{
                //    temp[x, y] = _block;
                //    return MoveNext(x, y, Direction.Right);
                //}
            }

            if (direction != Direction.Left && TryMoveRigth(x, y))
            {
                //if (!IsBlockWay(x, y))
                //{
                    return MoveNext(x, ++y, Direction.Right);
                //}
                //else
                //{
                //    temp[x, y] = _block;
                //    return MoveNext(x, y, Direction.Left);
                //}
            }

            if (direction != Direction.Up && TryMoveDown(x, y))
            {
                //if(!IsBlockWay(x, y))
                //{
                    return MoveNext(++x, y, Direction.Down);
                //}
                //else
                //{
                //    temp[x, y] = _block;
                //    return MoveNext(x, y, Direction.Up);
                //}
            }

            if (direction != Direction.Down && TryMoveUp(x, y))
            {
                //if (!IsBlockWay(x, y))
                //{
                    return MoveNext(--x, y, Direction.Up);
                //}
                //else
                //{
                //    temp[x, y] = _block;
                //    return MoveNext(x, y, Direction.Down);
                //}
            }

            return result;
        }
        
        private bool TryMoveLeft(int i, int j)
        {
            if (j > 0)
            {
                if (temp[i, --j] == _space)
                {
                    temp[i, j] = ++_step;
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
                    temp[i, j] = ++_step;
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
                    temp[i, j] = ++_step;
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
                    temp[i, j] = ++_step;
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
