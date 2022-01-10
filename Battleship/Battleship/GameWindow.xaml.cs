using System;
using System.Diagnostics;
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
        private bool shadowExists;
        private bool player1Coming;
        private int playerChangeCounter = 0;
        public delegate string Hit(int cell);
        public event Hit OnHit;

        public GameWindow(string player1, Grid playfield, char[,] playerTable)
        {
            InitializeComponent();

            _player1Name = player1;
            _player2Name = "AI";
            _rounds = 0;
            _player1Hits = 0;
            _player2Hits = 0;
            _playerTable = playerTable;
            player1Coming = SharedUtility.WhichPlayerStart();
            LoadPlayerTable(playfield);
            AI.GenerateAItable(rnd, _aiTable, rightTable);
            SharedUtility.ShipStatHpInit(carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);

            OnHit += new Hit(this.OnShoot);
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

        private bool IsHitShipUnit(int cell)
        {
            return char.IsDigit(_aiTable[cell / SharedUtility.ROWS, cell % SharedUtility.COLUMNS]);
        }

        public string OnShoot(int cell)
        {
            bool isHit = IsHitShipUnit(cell);

            SharedUtility.SetShipUnit(cell, isHit, true, leftTable, rightTable);

            if (isHit)
            {
                HitsLabelChange();
                return _aiTable[cell / SharedUtility.ROWS, cell % SharedUtility.COLUMNS].ToString();
            }

            player1Coming = !player1Coming;
            SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter);

            return "false";
        }

        private void HitsLabelChange()
        {
            if (player1Coming)
            {
                playerHitsLabel.Content = _player1Hits;
            }
            else
            {
                aiHitsLabel.Content = _player2Hits;
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
            if (e.ClickCount == 1)
            {
                int shipLength = 1;
                DeleteShadow();
                shadowExists = false;

                int cell = SharedUtility.CalculateCell(rightTable);

                for (int i = 0; i < shipLength; i++)
                {
                    if (char.IsDigit(_aiTable[cell % SharedUtility.COLUMNS, cell / SharedUtility.ROWS]))
                    {
                        Rectangle ship = AI.CreateShip(rightTable);
                        ship.Fill = Brushes.DarkRed;
                        Grid.SetRow(ship, cell / SharedUtility.ROWS);
                        Grid.SetColumn(ship, cell % SharedUtility.COLUMNS);

                        char c = _aiTable[cell % SharedUtility.COLUMNS, cell / SharedUtility.ROWS];

                        _aiTable[cell % SharedUtility.COLUMNS, cell / SharedUtility.ROWS] = 'H';

                        string shipUnitName = OnHit(cell);
                        SharedUtility.ShipHpDecrement(shipUnitName, carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);

                        ship.Visibility = Visibility.Visible;
                        rightTable.Children.Add(ship);

                        _player1Hits++;
                        playerHitsLabel.Content = _player1Hits;
                    }
                    else if (_aiTable[cell % SharedUtility.COLUMNS, cell / SharedUtility.ROWS] is not ('H' or 'M'))
                    {
                        Rectangle ship = AI.CreateShip(rightTable);
                        ship.Fill = Brushes.Gray;
                        Grid.SetRow(ship, cell / SharedUtility.ROWS);
                        Grid.SetColumn(ship, cell % SharedUtility.COLUMNS);

                        _aiTable[cell % SharedUtility.COLUMNS, cell / SharedUtility.ROWS] = 'M';

                        ship.Visibility = Visibility.Visible;
                        rightTable.Children.Add(ship);

                        SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter);

                        //AI logic
                    }
                }
            }
        }
    }
}
