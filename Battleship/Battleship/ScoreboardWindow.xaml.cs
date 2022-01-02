using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for ScoreboardWindow.xaml
    /// </summary>
    public partial class ScoreboardWindow : Window
    {
        public List<Game> AllGames { get; set; }

        public ScoreboardWindow()
        {
            InitializeComponent();

            using (GameDbContext _context = new())
            {
               AllGames = _context.Games.ToList();
            }

            GamesList.ItemsSource = AllGames;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }

    }
}
