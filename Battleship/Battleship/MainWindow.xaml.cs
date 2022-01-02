using System.Windows;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void VsAIButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerNameWindow playerName = new();
            Close();
            playerName.Show();
        }

        private void VsPlayerButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerName2Window playerName2 = new();
            Close();
            playerName2.Show();
        }

        private void ScoreboardButton_Click(object sender, RoutedEventArgs e)
        {
            ScoreboardWindow scoreboard = new();
            Close();
            scoreboard.Show();
        }
    }
}
