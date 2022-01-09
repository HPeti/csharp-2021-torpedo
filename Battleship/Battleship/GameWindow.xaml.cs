using System;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private readonly string _player1Name;
        private readonly string _player2Name;
        private int _rounds;
        private int _player1Hits;
        private int _player2Hits;
        private string _winner;
        private char[,] _aiTable = new char[10, 10];
        private char[,] _playerTable = new char[10, 10];

        Random rnd = new();

        public GameWindow(string player1, Grid playfield, char[,] playerTable)
        {
            InitializeComponent();

            _player1Name = player1;
            _player2Name = "AI";
            _rounds = 0;
            _player1Hits = 0;
            _player2Hits = 0;
            _playerTable = playerTable;
            LoadPlayerTable(playfield);
            AI.GenerateAItable(rnd, _aiTable, rightTable);
        }

        private void LoadPlayerTable(Grid playfield)
        {
            for (int ship = playfield.Children.Count -1; ship >= 0; ship--)
            {
                UIElement child = playfield.Children[ship];
                playfield.Children.RemoveAt(ship);
                leftTable.Children.Add(child);
            }
        }

      

        private void SurrendButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            Close();
            main.Show();
        }
    }
}
