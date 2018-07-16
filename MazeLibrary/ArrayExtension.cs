namespace MazeLibrary
{
    public static class ArrayExtension
    {
        public static int[,] Copy(this int[,] array)
        {
            int n = array.GetLength(0);
            int m = array.GetLength(1);

            int[,] result = new int[n, m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    result[i, j] = array[i, j];
                }
            }

            return result;
        }
    }
}
