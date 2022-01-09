using System.Windows;
using System.Windows.Input;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : Window
    {
        private string player1Name;
        private string player2Name;
        private bool versusComputer;

        /// <summary>
        /// This constructor is used when only one player plays the game versus the computer.
        /// </summary>
        /// <param name="player1Name">Player's name</param>
        public ShipPlacement(string player1Name)
        {
            InitializeComponent();

            versusComputer = true;
            this.player1Name = player1Name;

            this.Title = player1Name + "'s ship placement";
            welcomeLabel.Content = player1Name + "'s ship placement";
        }

        /// <summary>
        /// This constructor is used when two players plays the game.
        /// Firstly the player1's ships are set up.
        /// </summary>
        /// <param name="player1Name">First player's name</param>
        /// <param name="player2Name">Second player's name</param>
        public ShipPlacement(string player1Name, string player2Name)
        {
            InitializeComponent();

            versusComputer = false;
            this.player1Name = player1Name;
            this.player2Name = player2Name;

            this.Title = player1Name + "'s ship placement";
            welcomeLabel.Content = player1Name + "'s ship placement";
        }

        private void ShipBtn(object sender, RoutedEventArgs e)
        {
        }

        private void RotateBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OnGridMouseClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// This method navigates back to MainWindow when the user presses the backBtn.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            Close();
            mainWindow.Show();
        }

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new(player1Name);
            Close();
            game.Show();
        }
    }
}