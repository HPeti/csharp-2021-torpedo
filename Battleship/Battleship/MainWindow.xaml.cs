using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
