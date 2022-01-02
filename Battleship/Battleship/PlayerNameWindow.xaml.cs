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
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }
    }
}
