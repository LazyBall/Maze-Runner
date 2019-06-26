using System.Windows;

namespace Maze_Runner
{
    /// <summary>
    /// Логика взаимодействия для SavingRecordWindow.xaml
    /// </summary>
    partial class SavingResultWindow : Window
    {
        public SavingResultWindow()
        {
            InitializeComponent();
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (PlayersName == string.Empty)
            {
                MessageBox.Show("Name is empty!");
                return;
            }
            this.DialogResult = true;
        }

        public string PlayersName
        {
            get { return Name_Textbox.Text; }
        }
    }
}