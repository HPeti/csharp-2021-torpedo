using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
        private bool shipVisibility;
        private int calculatedCell = -1;
        private bool shadowExists = false;

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

        private Rectangle CreateShadow()
        {
            Rectangle shadow = new Rectangle
            {
                Fill = Brushes.LightGray
            };
            double X = rightTable.Height / SharedUtility.COLUMNS;
            double Y = rightTable.Width / SharedUtility.ROWS;

            shadow.Height = X;
            shadow.Width = Y;

            return shadow;
        }

        private void DeleteShadow()
        {
            if (shadowExists)
            {
                int lastItem = rightTable.Children.Count - 1;
                rightTable.Children.RemoveAt(lastItem);
            }
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e)
        {
            int cell = SharedUtility.CalculateCell(rightTable);

            if (calculatedCell != cell)
            {
                calculatedCell = cell;

                DeleteShadow();

                Rectangle shadow = CreateShadow();

                Grid.SetRow(shadow, cell / SharedUtility.ROWS);
                Grid.SetColumn(shadow, cell % SharedUtility.COLUMNS);

                rightTable.Children.Add(shadow);
                shadowExists = true;
            }
        }

        private void SurrendButton_Click(object sender, RoutedEventArgs e)
        {
            DbHelper.InsertToDb(_player1Name, _player2Name, _rounds, _player1Hits, _player2Hits, _player2Name);

            MainWindow main = new();
            Close();
            main.Show();
        }

        private void ChangeShipVisibility_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F8))
            {
                shipVisibility = !shipVisibility;

                for (int i = 0; i < 15; i++)
                {
                    rightTable.Children[i].Visibility = shipVisibility ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        private void OnGridMouseClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
