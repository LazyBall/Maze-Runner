using System;
using System.Windows.Input;

namespace Maze_Runner
{
    [Serializable]
    public class Settings
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public Key Up { get; set; }
        public Key Down { get; set; }
        public Key Left { get; set; }
        public Key Right { get; set; }
        public Key Zoom_In { get; set; }
        public Key Zoom_Out { get; set; }
        public Settings()
        {
            Height = 25;
            Width = 25;
            Up = Key.Up;
            Down = Key.Down;
            Left = Key.Left;
            Right = Key.Right;
            Zoom_In = Key.Add;
            Zoom_Out=Key.Subtract;
        }
    }
}