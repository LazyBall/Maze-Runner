using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Maze_Runner
{
    /// <summary>
    /// Логика взаимодействия для WindowGame2.xaml
    /// </summary>
    public partial class WindowGame2 : Window
    {
        Maze _maze;
        int _rows = 15;
        int _colums = 15;
        SolidColorBrush _wall = Brushes.Black;
        SolidColorBrush _free = Brushes.Gray;
        double _height = 25;
        double _width = 25;
        int mob = 1;
        Rectangle _exit = new Rectangle()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Fill = Brushes.Green,
            Stroke = Brushes.Black,
        };
        Image _hero = new Image
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Source = new BitmapImage(new Uri("hero.png", UriKind.Relative)),
        };
        Settings _settings;
        System.Drawing.Point[] evils;
        Image[] evil;
        string[] direction_mob;

        public WindowGame2()
        {
            _settings = Serializer.DeserializeSettings();
            InitializeComponent();
            GoToNextLevel();

        }

        private void GoToNextLevel()
        {
            _maze = Maze.GenerateMaze(_rows, _colums);
            evils = Mobs.GenerationMobs(_maze, mob);
            evil = new Image[mob];
            direction_mob = new string[mob];
            _maze.OnExit += Win;
            Draw();
        }

        private void StepMob()
        {
            for (int i = 0; i < mob; i++)
            {
                Random random = new Random();
                bool fl = true;
                int count = 0;
                while (fl)
                {
                    int dir = random.Next(1, 5);
                    count++;
                    switch (dir)
                    {
                        case (1):
                            {
                                if (direction_mob[i].CompareTo("Up") != 0 && _maze.Matrix[evils[i].X - 1, evils[i].Y])
                                {
                                    direction_mob[i] = "Down";
                                    evils[i].X--;
                                    evil[i].Margin = new Thickness(evils[i].Y * _width, evils[i].X * _height, 0, 0);
                                    fl = false;
                                }
                                break;
                            }
                        case (2):
                            {
                                if (direction_mob[i].CompareTo("Down") != 0 && _maze.Matrix[evils[i].X + 1, evils[i].Y])
                                {
                                    direction_mob[i] = "Up";
                                    evils[i].X++;
                                    evil[i].Margin = new Thickness(evils[i].Y * _width, evils[i].X * _height, 0, 0);
                                    fl = false;
                                }
                                break;
                            }
                        case (3):
                            {
                                if (direction_mob[i].CompareTo("Left") != 0 && _maze.Matrix[evils[i].X, evils[i].Y - 1])
                                {
                                    direction_mob[i] = "Right";
                                    evils[i].Y--;
                                    evil[i].Margin = new Thickness(evils[i].Y * _width, evils[i].X * _height, 0, 0);
                                    fl = false;
                                }
                                break;
                            }
                        case (4):
                            {
                                if (direction_mob[i].CompareTo("Right") != 0 && _maze.Matrix[evils[i].X, evils[i].Y + 1])
                                {
                                    direction_mob[i] = "Left";
                                    evils[i].Y++;
                                    evil[i].Margin = new Thickness(evils[i].Y * _width, evils[i].X * _height, 0, 0);
                                    fl = false;
                                }
                                break;
                            }
                    }
                    if (count == 10)
                        direction_mob[i] = "";
                }
            }
        }

        private void Draw()
        {
            GameField.Children.Clear();
            for (int i = 0; i < _maze.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < _maze.Matrix.GetLength(1); j++)
                {
                    var rect = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Height = _height,
                        Width = _width,
                        Stroke = Brushes.Black,
                        Margin = new Thickness(j * _width, i * _height, 0, 0),
                    };
                    if (_maze.Matrix[i, j])
                    {
                        rect.Fill = _free;
                    }
                    else rect.Fill = _wall;
                    GameField.Children.Add(rect);
                }
            }
            for (int i = 0; i < mob; i++)
            {
                direction_mob[i] = "";
                evil[i] = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Source = new BitmapImage(new Uri("evil.png", UriKind.Relative)),
                    Height = _height,
                    Width = _width,
                    Margin = new Thickness(evils[i].Y * _width, evils[i].X * _height, 0, 0),
                };
                GameField.Children.Add(evil[i]);
            }
            _exit.Height = _height;
            _exit.Width = _width;
            _exit.Margin = new Thickness(_maze.Exit.Y * _width, _maze.Exit.X * _height, 0, 0);
            GameField.Children.Add(_exit);
            _hero.Height = _height;
            _hero.Width = _width;
            _hero.Margin = Margin = new Thickness(_maze.PlayersPosition.Y * _width, _maze.PlayersPosition.X * _height, 0, 0);
            GameField.Children.Add(_hero);
        }

        private void Win(object sender, EventArgs e)
        {
            _rows += 2;
            _colums += 2;
            mob++;
            GoToNextLevel();
        }

        private void GameField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == _settings.Up)
            {
                _maze.MoveTo(Directions.Up);
                Cheker();
                StepMob();
                Cheker();
            }
            else if (e.Key == _settings.Down)
            {
                _maze.MoveTo(Directions.Down);
                Cheker();
                StepMob();
                Cheker();
            }
            else if (e.Key == _settings.Left)
            {
                _maze.MoveTo(Directions.Left);
                Cheker();
                StepMob();
                Cheker();
            }
            else if (e.Key == _settings.Right)
            {
                _maze.MoveTo(Directions.Right);
                Cheker();
                StepMob();
                Cheker();
            }
            else if (e.Key == _settings.Zoom_In)
            {
                _height = _height + _height * (0.1);
                _width = _width + _width * (0.1);
                Draw();
                return;
            }
            else if (e.Key == _settings.Zoom_Out)
            {
                _height = _height - _height * (0.1);
                _width = _width - _width * (0.1);
                Draw();
                return;
            }
            else return;
            _hero.Margin = new Thickness(_maze.PlayersPosition.Y * _width,
               _maze.PlayersPosition.X * _height, 0, 0);
        }
        private void Cheker()
        {
            for (int i = 0; i < mob; i++)
                if (_maze.PlayersPosition == evils[i])
                {
                    MessageBox.Show("You DIE");
                    ScrolGameField.Visibility = Visibility.Hidden;
                    this.Close();
                }
        }
    }


    public static class Mobs
    {
        public static System.Drawing.Point[] GenerationMobs(Maze maze, int count)
        {
            System.Drawing.Point[] posmobs = new System.Drawing.Point[count];
            var ex = maze.Exit;
            var hero = maze.PlayersPosition;
            var matrix = maze.Matrix;
            Random random = new Random();
            while (count != 0)
            {
                int x = random.Next(0, Convert.ToInt32(Math.Sqrt(matrix.Length)));
                int y = random.Next(0, Convert.ToInt32(Math.Sqrt(matrix.Length)));
                if (matrix[x, y] == true && Rad(x, y, ex) > 25 && Rad(x, y, hero) > 25)
                {
                    posmobs[count - 1] = new System.Drawing.Point(x, y);
                    count--;
                }
            }
            return posmobs;
        }
        private static double Rad(int x, int y, System.Drawing.Point p)
        {
            return (Math.Pow((p.X - x), 2) + Math.Pow((p.Y - y), 2));
        }
    }
}