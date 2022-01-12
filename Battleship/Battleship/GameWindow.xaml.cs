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
        private char[,] _aiTable = new char[10, 10];
        private char[,] _playerTable = new char[10, 10];

        private Random rnd = new();
        private bool shipVisibility;
        private int calculatedCell = -1;
        private bool shadowExists;
        private bool player1Coming;
        private int playerChangeCounter;
        private bool up, down, left, right, con, isHit;
        private int randomX, randomY, firstX, firstY;

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
            SharedUtility.PlayerShipsLoad(playfield, leftTable);
            AI.GenerateAItable(rnd, _aiTable, rightTable);
            SharedUtility.ShipStatHpInit(carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);

            if (!player1Coming)
            {
                Logic();
            }
        }

        private Rectangle ShipSettings()
        {
            Rectangle ship = new()
            {
                Fill = Brushes.Green
            };

            double x = rightTable.Height / SharedUtility.COLUMNS;
            double y = rightTable.Width / SharedUtility.ROWS;

            ship.Height = x;
            ship.Width = y;

            return ship;
        }

        private void AIHitsLabelChange()
        {
            _player2Hits++;
            aiHitsLabel.Content = _player2Hits;
        }

        private void InitDirection()
        {
            up = false;
            down = false;
            left = false;
            right = false;
        }

        private void DeleteShadow()
        {
            if (shadowExists)
            {
                int lastItem = rightTable.Children.Count - 1;
                rightTable.Children.RemoveAt(lastItem);
            }
        }

        private void ShootedCellChange(int x, int y, bool isHit)
        {
            _playerTable[x, y] = isHit ? 'H' : 'M';
        }

        private void PaintMissCell(int x, int y)
        {
            Rectangle ship = ShipSettings();
            ship.Fill = Brushes.Gray;
            Grid.SetRow(ship, x);
            Grid.SetColumn(ship, y);

            leftTable.Children.Add(ship);
        }

        private void PaintHitCell(int x, int y)
        {
            Rectangle ship = ShipSettings();
            ship.Fill = Brushes.DarkRed;

            Grid.SetRow(ship, x);
            Grid.SetColumn(ship, y);

            leftTable.Children.Add(ship);
        }

        private bool ShipDestroyed(bool up, bool down, bool left, bool right)
        {
            if (up && down && left && right)
            {
                InitDirection();
                con = false;

                return true;
            }

            return false;
        }

        private bool Shoot(int randomX, int randomY, string direction)
        {
            switch (direction)
            {
                case "Up":
                    randomY++;
                    break;
                case "Down":
                    randomY--;
                    break;
                case "Left":
                    randomX--;
                    break;
                case "Right":
                    randomX++;
                    break;
                default:
                    break;
            }

            if (!AI.DetectBorder(randomX, randomY))
            {
                if (!AI.IsShootedCell(randomX, randomY, _playerTable))
                {
                    if (AI.IsPlayerUnit(randomX, randomY, _playerTable))
                    {
                        //Debug.WriteLine("AI shooted at x:{0}, y:{1}", randomX, randomY);
                        ShootedCellChange(randomX, randomY, true);
                        PaintHitCell(randomX, randomY);

                        return true;
                    }
                    else
                    {
                        //Debug.WriteLine("AI missed at x:{0}, y:{1}", randomX, randomY);
                        ShootedCellChange(randomX, randomY, false);
                        PaintMissCell(randomX, randomY);

                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Logic()
        {
            isHit = false;
            while (!player1Coming)
            {
                if (!con)
                {
                    int cell = AI.GenerateShoot(rnd, _playerTable);
                    randomY = cell / SharedUtility.COLUMNS;
                    randomX = cell % SharedUtility.ROWS;

                    isHit = Shoot(randomX, randomY, "center");

                    if (isHit)
                    {
                        AIHitsLabelChange();
                        firstX = randomX;
                        firstY = randomY;
                    }
                }
                else
                {
                    isHit = true;
                }

                while (isHit)
                {
                    con = true;

                    int direction = rnd.Next(0, 4);

                    switch (direction)
                    {
                        case 0:
                            while (!up)
                            {
                                if (Shoot(randomX, randomY, "Up"))
                                {
                                    randomY++;
                                    AIHitsLabelChange();
                                    right = true;
                                    left = true;
                                }
                                else
                                {
                                    randomY = firstY;
                                    player1Coming = true;
                                    isHit = false;
                                    up = true;
                                    SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref _rounds);
                                }
                            }
                            break;
                        case 1:
                            while (!down)
                            {
                                if (Shoot(randomX, randomY, "Down"))
                                {
                                    randomY--;
                                    right = true;
                                    left = true;
                                    AIHitsLabelChange();
                                }
                                else
                                {
                                    randomY = firstY;
                                    player1Coming = true;
                                    isHit = false;
                                    down = true;
                                    SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref _rounds);
                                }
                            }
                            break;
                        case 2:
                            while (!left)
                            {
                                if (Shoot(randomX, randomY, "Left"))
                                {
                                    randomX--;
                                    up = true;
                                    down = true;
                                    AIHitsLabelChange();
                                }
                                else
                                {
                                    randomX = firstX;
                                    player1Coming = true;
                                    isHit = false;
                                    left = true;
                                    SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref _rounds);
                                }
                            }
                            break;
                        case 3:
                            while (!right)
                            {
                                if (Shoot(randomX, randomY, "Right"))
                                {
                                    randomX++;
                                    up = true;
                                    down = true;
                                    AIHitsLabelChange();
                                }
                                else
                                {
                                    randomX = firstX;
                                    player1Coming = true;
                                    isHit = false;
                                    right = true;
                                    SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref _rounds);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if (ShipDestroyed(up, down, left, right))
                    {
                        con = false;
                        break;
                    }

                }
                if (!isHit)
                {
                    player1Coming = true;
                }
            }
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e)
        {
            int cell = SharedUtility.CalculateCell(rightTable);

            if (calculatedCell != cell)
            {
                calculatedCell = cell;

                DeleteShadow();

                Rectangle shadow = ShipSettings();
                shadow.Fill = Brushes.Gray;

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

                        SharedUtility.ShipHpDecrement(c.ToString(), carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);

                        ship.Visibility = Visibility.Visible;
                        rightTable.Children.Add(ship);

                        _player1Hits++;
                        playerHitsLabel.Content = _player1Hits;

                        if (_player1Hits == 15)
                        {
                            SharedUtility.EndGame(_player1Name, _player2Name, _rounds, _player1Hits, _player2Hits, _player1Name);
                            MainWindow main = new();
                            Close();
                            main.Show();
                        }
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

                        SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref _rounds);
                        player1Coming = false;
                        Logic();

                        if (_player2Hits == 15)
                        {
                            SharedUtility.EndGame(_player1Name, _player2Name, _rounds, _player1Hits, _player2Hits, _player2Name);
                            MainWindow main = new();
                            Close();
                            main.Show();
                        }
                    }
                }
            }
        }
    }
}