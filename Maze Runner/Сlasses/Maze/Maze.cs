using System;
using System.Collections.Generic;
using System.Drawing;

namespace Maze_Runner
{
    public enum Directions
    {
        Left,
        Right,
        Up,
        Down,
    }

    public class Maze
    {
        public event EventHandler OnExit;
        public Point PlayersPosition { get; private set; }
        public Point Exit { get; private set; }
        public bool[,] Matrix { get; private set; }

        public Maze(bool[,] matrix, Point enter, Point exit)
        {
            CheckInputParameters(matrix, enter, exit);
            FillMatrix(matrix);
            PlayersPosition = new Point(enter.X + 1, enter.Y + 1);
            Exit = new Point(exit.X + 1, exit.Y + 1);
            GetDistanceToExit();
        }

        private bool CheckInputParameters(bool[,] matrix, Point enter, Point exit)
        {
            int count = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j]) count++;
                    if (count > 1) break;
                }
            }
            if (count < 2) throw new
                    ArgumentException("В матрице должно быть хотя бы две свободные точки");
            if (!matrix[enter.X, enter.Y]) throw new
                    ArgumentException("Вход (начальная позиция игрока) должен быть свободен");
            if (!matrix[exit.X, exit.Y]) throw new
                    ArgumentException("Выход должен быть свободен");
            if (enter == exit) throw new
                    ArgumentException("Вход (начальная позиция игрока) и выход не должны совпадать");
            return true;
        }

        private void FillMatrix(bool[,] matrix)
        {
            int rows = matrix.GetLength(0) + 2;
            int columns = matrix.GetLength(1) + 2;
            Matrix = new bool[rows, columns];
            for (int i = 1; i < rows - 1; i++)
            {
                for (int j = 1; j < columns - 1; j++)
                {
                    Matrix[i, j] = matrix[i - 1, j - 1];
                }
            }
        }

        public int GetDistanceToExit()
        {
            int distance = 0;
            var added = new HashSet<Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(PlayersPosition);
            added.Add(PlayersPosition);
            var neighbors = new List<Point>();
            while (queue.Count > 0)
            {
                int count = queue.Count;
                for (int i = 0; i < count; i++)
                {
                    var point = queue.Dequeue();
                    if (point == Exit) return distance;
                    neighbors.Add(new Point(point.X - 1, point.Y));
                    neighbors.Add(new Point(point.X + 1, point.Y));
                    neighbors.Add(new Point(point.X, point.Y + 1));
                    neighbors.Add(new Point(point.X, point.Y - 1));
                    foreach (var neighbor in neighbors)
                    {
                        if (Matrix[neighbor.X, neighbor.Y] && !added.Contains(neighbor))
                        {
                            queue.Enqueue(neighbor);
                            added.Add(neighbor);
                        }
                    }
                    neighbors.Clear();
                }
                distance++;
            }
            throw new ArgumentException("Выход недостижим");
        }

        private bool CheckMoving(int down, int right)
        {
            if (Matrix[PlayersPosition.X + down, PlayersPosition.Y + right])
            {
                if (PlayersPosition.X + down == Exit.X &&
                    PlayersPosition.Y + right == Exit.Y)
                {
                    if(OnExit!=null)
                    {
                        OnExit.Invoke(new object(), new EventArgs());
                    }
                }
                return true;
            }
            return false;
        }

        public void MoveTo(Directions direction)
        {
            switch (direction)
            {
                case (Directions.Left):
                    {
                        if (CheckMoving(0, -1))
                            PlayersPosition = new Point(PlayersPosition.X,
                                PlayersPosition.Y - 1);
                        break;
                    }
                case (Directions.Right):
                    {
                        if (CheckMoving(0, 1))
                            PlayersPosition = new Point(PlayersPosition.X,
                                PlayersPosition.Y + 1);
                        break;
                    }
                case (Directions.Up):
                    {
                        if (CheckMoving(-1, 0))
                            PlayersPosition = new Point(PlayersPosition.X - 1,
                                PlayersPosition.Y);
                        break;
                    }
                case (Directions.Down):
                    {
                        if (CheckMoving(1, 0))
                            PlayersPosition = new Point(PlayersPosition.X + 1,
                                PlayersPosition.Y);
                        break;
                    }
            }
        }

        public static Maze GenerateMaze(int rows, int columns)
        {
            var random = new Random(DateTime.Now.Millisecond);
            bool[,] matrix;
            switch (random.Next(4))          
            {
                case (0):
                    {
                        matrix = GenerationAlgorithms.RunBinaryTreeAlgorithm(rows, columns);
                        break;
                    }
                case (1):
                    {
                        matrix = GenerationAlgorithms.RunSidewinderAlgorithm(rows, columns);
                        break;
                    }
                case (2):
                    {
                        matrix = GenerationAlgorithms.RunAldous_BroderAlgorithm(rows, columns);
                        break;
                    }
                default:
                    {
                        matrix = GenerationAlgorithms.RunWilsonAlgorithm(rows, columns);
                        break;
                    }
            }
            var enter = GetEnter(matrix);
            var exit = GetExit(matrix, enter);
            return new Maze(matrix, enter, exit);
        }

        private static Point GetEnter(bool[,] matrix)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            Point enter;
            do
            {
                enter = new Point(random.Next(rows), random.Next(columns));
            } while (!matrix[enter.X, enter.Y]);
            return enter;
        }

        private static Point GetExit(bool[,] matrix, Point enter)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);
            var added = new HashSet<Point>();
            var queue = new Queue<Point>();
            queue.Enqueue(enter);
            added.Add(enter);
            Point point = enter;
            while (queue.Count > 0)
            {
                point = queue.Dequeue();
                var right = new Point(point.X, point.Y + 1);
                var left = new Point(point.X, point.Y - 1);
                var up = new Point(point.X - 1, point.Y);
                var down = new Point(point.X + 1, point.Y);
                if (right.Y < columns && matrix[right.X, right.Y] &&
                    !added.Contains(right))
                {
                    queue.Enqueue(right);
                    added.Add(right);
                }
                if (left.Y >= 0 && matrix[left.X, left.Y] && !added.Contains(left))
                {
                    queue.Enqueue(left);
                    added.Add(left);
                }
                if (up.X >= 0 && matrix[up.X, up.Y] && !added.Contains(up))
                {
                    queue.Enqueue(up);
                    added.Add(up);
                }
                if (down.X < rows && matrix[down.X, down.Y] && !added.Contains(down))
                {
                    queue.Enqueue(down);
                    added.Add(down);
                }
            }
            return point;
        }
    }
}