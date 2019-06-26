using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Maze_Runner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    partial class MainWindow : Window
    {
        Maze _maze;
        int _rows = 15;
        int _colums = 15;
        byte _stepGrowthColumns = 2;
        byte _stepGrowthRows = 2;
        SolidColorBrush _wall = Brushes.Black;
        SolidColorBrush _free = Brushes.Gray;
        double _height;
        double _width;
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
        DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = new TimeSpan(0, 0, 1),
        };
        int _time;
        int _level;

        public MainWindow()
        {
            _settings = Serializer.DeserializeSettings();
            _timer.Tick += _timer_Tick;
            InitializeComponent();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _time--;
            Label_Time.Content = "Time: " + _time;
            if (_time == 0)
            {
                _timer.Stop();
                GoToSaveResultDialog();
                Play_Panel.Visibility = Visibility.Hidden;
                Menu_Grid.Visibility = Visibility.Visible;
            }
        }

        private void Menu_Grid_Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            WindowGame.Close();
        }

        #region Play

        private void Menu_Grid_Play_Button_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Second version of the game?", "Version", MessageBoxButton.YesNo) ==
            //    MessageBoxResult.Yes)
            //{
            //    new WindowGame2().Show();
            //    return;
            //}
            _level = 0;
            _height = _settings.Height;
            _width = _settings.Width;
            GoToNextLevel();
            Menu_Grid.Visibility = Visibility.Hidden;
            Play_Panel.Visibility = Visibility.Visible;
        }

        private void GoToNextLevel()
        {
            _maze = Maze.GenerateMaze(_rows, _colums);
            _maze.OnExit += Win;
            _time = _maze.GetDistanceToExit() / 2;
            _level++;
            Label_Time.Content = "Time: " + _time;
            Label_Level.Content = "Level: " + _level;
            Draw();
            _timer.Start();
        }

        private void Draw()
        {
            GameField_Grid.Children.Clear();
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
                    GameField_Grid.Children.Add(rect);
                }
            }
            _exit.Height = _height;
            _exit.Width = _width;
            _exit.Margin = new Thickness(_maze.Exit.Y * _width, _maze.Exit.X * _height, 0, 0);
            GameField_Grid.Children.Add(_exit);
            _hero.Height = _height;
            _hero.Width = _width;
            _hero.Margin = Margin = new Thickness(_maze.PlayersPosition.Y * _width, _maze.PlayersPosition.X * _height, 0, 0);
            GameField_Grid.Children.Add(_hero);
        }

        private void Win(object sender, EventArgs e)
        {
            _timer.Stop();
            _rows += _stepGrowthRows;
            _colums += _stepGrowthColumns;
            MessageBox.Show("Are you ready?", "Next Level", MessageBoxButton.OK);
            GoToNextLevel();
        }

        private void GameField_Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GameField_Grid.IsVisible) return;
            if (e.Key == _settings.Up)
            {
                _maze.MoveTo(Directions.Up);
            }
            else if (e.Key == _settings.Down)
            {
                _maze.MoveTo(Directions.Down);
            }
            else if (e.Key == _settings.Left)
            {
                _maze.MoveTo(Directions.Left);
            }
            else if (e.Key == _settings.Right)
            {
                _maze.MoveTo(Directions.Right);
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

        #endregion

        #region Settings

        private void Menu_Grid_Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Setting_Up_Button.Content = _settings.Up.ToString();
            Setting_Down_Button.Content = _settings.Down.ToString();
            Setting_Right_Button.Content = _settings.Right.ToString();
            Setting_Left_Button.Content = _settings.Left.ToString();
            Setting_Zoom_In_Button.Content = _settings.Zoom_In.ToString();
            Setting_Zoom_Out_Button.Content = _settings.Zoom_Out.ToString();
            Menu_Grid.Visibility = Visibility.Hidden;
            Settings_Grid.Visibility = Visibility.Visible;
        }

        private void Setting_Any_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ((Button)sender).Content = "";
            }
        }

        private void Setting_Up_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Up = e.Key;
            UpdateContent(sender, e);
        }

        private void Setting_Down_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Down = e.Key;
            UpdateContent(sender, e);
        }

        private void Setting_Left_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Left = e.Key;
            UpdateContent(sender, e);
        }

        private void Setting_Right_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Right = e.Key;
            UpdateContent(sender, e);
        }

        private void Setting_Zoom_In_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Zoom_In = e.Key;
            UpdateContent(sender, e);
        }

        private void Setting_Zoom_Out_Button_KeyDown(object sender, KeyEventArgs e)
        {
            _settings.Zoom_Out = e.Key;
            UpdateContent(sender, e);
        }

        private void UpdateContent(object sender, KeyEventArgs e)
        {
            if (sender is Button)
            {
                ((Button)sender).Content = e.Key.ToString();
            }
        }

        private void Settings_Grid_Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((new HashSet<Key>() { _settings.Up, _settings.Down, _settings.Right, _settings.Left,
                _settings.Zoom_In, _settings.Zoom_Out}).Count == 6)
            {
                Serializer.SerializeSettings(_settings);
            }
            else
            {
                _settings = Serializer.DeserializeSettings();
            }
            Settings_Grid.Visibility = Visibility.Hidden;
            Menu_Grid.Visibility = Visibility.Visible;
        }

        private void Settings_Grid_Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            _settings = Serializer.DeserializeSettings();
            Settings_Grid.Visibility = Visibility.Hidden;
            Menu_Grid.Visibility = Visibility.Visible;
        }

        #endregion

        #region Results

        private void Menu_Grid_Results_Button_Click(object sender, RoutedEventArgs e)
        {
            var results = Serializer.DeserializeResults();
            Results_Panel.Children.Clear();
            int i = 1;
            foreach (var item in results)
            {
                Results_Panel.Children.Add(new TextBlock()
                { Text = i + ". " + "Name: " + item.PlayersName + "  Level: " + item.Level, FontSize = 30 });
                i++;
            }
            Menu_Grid.Visibility = Visibility.Hidden;
            Results_Grid.Visibility = Visibility.Visible;
        }

        private void GoToSaveResultDialog()
        {
            SavingResultWindow savingWindow = new SavingResultWindow();
            if (savingWindow.ShowDialog() == true)
            {
                var results = Serializer.DeserializeResults();
                results.Add(new Result() { PlayersName = savingWindow.PlayersName, Level = _level });
                Serializer.SerializeResults(results);
                MessageBox.Show("The result is saved!");
            }
        }

        private void Results_Grid_Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            Results_Grid.Visibility = Visibility.Hidden;
            Menu_Grid.Visibility = Visibility.Visible;
        }

        #endregion
    }
}