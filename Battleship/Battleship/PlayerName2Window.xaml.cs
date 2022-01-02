using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for PlayerName2Window.xaml
    /// </summary>
    public partial class PlayerName2Window : Window
    {
        public PlayerName2Window()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regexItem = new("^[a-zA-Z0-9 ]*$");

            if (string.IsNullOrWhiteSpace(player1Name.Text) || string.IsNullOrWhiteSpace(player2Name.Text) || !regexItem.IsMatch(player1Name.Text) || !regexItem.IsMatch(player2Name.Text) || string.Equals(player1Name.Text, player2Name.Text, StringComparison.Ordinal))
            {
                _ = MessageBox.Show("The given names are not valid!");
                player1Name.Text = "";
                player2Name.Text = "";
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
