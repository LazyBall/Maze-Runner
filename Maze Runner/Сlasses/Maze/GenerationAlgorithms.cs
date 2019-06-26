using System;
using System.Collections.Generic;
using System.Drawing;

namespace Maze_Runner
{
    public static class GenerationAlgorithms
    {
        public static bool[,] RunBinaryTreeAlgorithm(int rows, int columns)
        {
            var matrix = new bool[rows, columns];
            for (int i = 0; i < columns; i++)
            {
                matrix[0, i] = true;
            }
            for (int i = 0; i < rows; i++)
            {
                matrix[i, columns - 1] = true;
            }
            var random = new Random(DateTime.Now.Millisecond);
            for (int i = 2; i < rows; i += 2)
            {
                for (int j = 0; j < columns - 1; j += 2)
                {
                    matrix[i, j] = true;
                    if (random.Next(2) == 0)
                    {
                        matrix[i - 1, j] = true;
                    }
                    else matrix[i, j + 1] = true;
                }
            }
            return matrix;
        }


        public static bool[,] RunSidewinderAlgorithm(int rows, int columns)
        {
            var matrix = new bool[rows, columns];
            for (int i = 0; i < columns; i++)
            {
                matrix[0, i] = true;
            }
            var random = new Random(DateTime.Now.Millisecond);
            var list = new List<int>();
            for (int i = 2; i < rows; i += 2)
            {
                for (int j = 0; j < columns; j += 2)
                {
                    matrix[i, j] = true;
                    list.Add(j);
                    if (random.Next(2) == 0)
                    {
                        matrix[i - 1, list[random.Next(list.Count)]] = true;
                        list.Clear();
                    }
                    else
                    {
                        if (j + 2 < columns)
                        {
                            matrix[i, j + 1] = true;
                        }
                        else
                        {
                            if (j + 1 < columns)
                            {
                                matrix[i, j + 1] = true;
                            }
                            matrix[i - 1, list[random.Next(list.Count)]] = true;
                            list.Clear();
                        }
                    }
                }
            }
            return matrix;
        }

        public static bool[,] RunAldous_BroderAlgorithm(int rows, int columns)
        {
            var matrix = new bool[rows, columns];
            var visited = new HashSet<Point>();
            var random = new Random(DateTime.Now.Millisecond);
            var position = new Point(random.Next((rows + 1) / 2) * 2,
                random.Next((columns + 1) / 2) * 2);
            matrix[position.X, position.Y] = true;
            visited.Add(position);
            int count = ((rows + 1) / 2) * ((columns + 1) / 2);
            var neighbors = new List<Point>();
            while (visited.Count != count)
            {
                if (position.X - 2 >= 0)
                    neighbors.Add(new Point(position.X - 2, position.Y));
                if (position.X + 2 < rows)
                    neighbors.Add(new Point(position.X + 2, position.Y));
                if (position.Y - 2 >= 0)
                    neighbors.Add(new Point(position.X, position.Y - 2));
                if (position.Y + 2 < columns)
                    neighbors.Add(new Point(position.X, position.Y + 2));
                var nextPosition = neighbors[random.Next(neighbors.Count)];
                neighbors.Clear();
                if (!visited.Contains(nextPosition))
                {
                    visited.Add(nextPosition);
                    var wall = new Point
                    {
                        X = position.X + (nextPosition.X - position.X) / 2,
                        Y = position.Y + (nextPosition.Y - position.Y) / 2
                    };
                    matrix[wall.X, wall.Y] = true;
                    matrix[nextPosition.X, nextPosition.Y] = true;
                }
                position = nextPosition;
            }
            return matrix;
        }

        public static bool[,] RunWilsonAlgorithm(int rows, int columns)
        {
            var matrix = new bool[rows, columns];
            var mainTree = new HashSet<Point>();
            var random = new Random(DateTime.Now.Millisecond);
            var root = new Point(random.Next((rows + 1) / 2) * 2,
                random.Next((columns + 1) / 2) * 2);
            matrix[root.X, root.Y] = true;
            mainTree.Add(root);
            int count = ((rows + 1) / 2) * ((columns + 1) / 2);
            while (mainTree.Count != count)
            {
                var currentTree = new Stack<Point>();
                var visited = new HashSet<Point>();
                var position = new Point(random.Next((rows + 1) / 2) * 2,
                random.Next((columns + 1) / 2) * 2);
                var neighbors = new List<Point>();
                while (!mainTree.Contains(position))
                {
                    if (!visited.Contains(position))
                    {
                        currentTree.Push(position);
                        visited.Add(position);
                    }
                    else
                    {
                        while (currentTree.Peek() != position)
                        {
                            visited.Remove(currentTree.Pop());
                        }
                    }
                    if (position.X - 2 >= 0)
                        neighbors.Add(new Point(position.X - 2, position.Y));
                    if (position.X + 2 < rows)
                        neighbors.Add(new Point(position.X + 2, position.Y));
                    if (position.Y - 2 >= 0)
                        neighbors.Add(new Point(position.X, position.Y - 2));
                    if (position.Y + 2 < columns)
                        neighbors.Add(new Point(position.X, position.Y + 2));
                    var nextPosition = neighbors[random.Next(neighbors.Count)];
                    neighbors.Clear();
                    position = nextPosition;
                }
                while (currentTree.Count > 0)
                {
                    var previousPosition = currentTree.Pop();
                    var wall = new Point
                    {
                        X = position.X + (previousPosition.X - position.X) / 2,
                        Y = position.Y + (previousPosition.Y - position.Y) / 2
                    };
                    matrix[wall.X, wall.Y] = true;
                    matrix[previousPosition.X, previousPosition.Y] = true;
                    mainTree.Add(previousPosition);
                    position = previousPosition;
                }
            }
            return matrix;
        }
    }
}