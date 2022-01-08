using System.Text.RegularExpressions;
using System.Windows;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for PlayerNameWindow.xaml
    /// </summary>
    public partial class PlayerNameWindow : Window
    {
        public PlayerNameWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regexItem = new("^[a-zA-Z0-9 ]*$");

            if (string.IsNullOrWhiteSpace(playerName.Text) || !regexItem.IsMatch(playerName.Text))
            {
                _ = MessageBox.Show("Player's name is not valid!");
                playerName.Text = "";
            }
            else
            {
                ShipPlacement shipPlacement = new(playerName.Text);
                Close();
                shipPlacement.Show();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }
    }
}
