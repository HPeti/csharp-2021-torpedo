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
    /// Interaction logic for ShipPlacement.xaml
    /// </summary>
    public partial class ShipPlacement : Window
    {
        private const int ROW_NUM = SharedUtility.ROWS;
        private const int COL_NUM = SharedUtility.COLUMNS;
        private Brush shadowFillBrush = Brushes.LightBlue;
        private Brush shipFillBrush = Brushes.DarkBlue;

        private string selectedShip = null;
        private char selectedShipUnit;
        private int calculatedCell = -1;
        private bool shipShadow = false;
        private bool shipHorizontal = false;

        private char[,] battleshipPlayfield = new char[ROW_NUM, COL_NUM];

        private bool vsComputer;
        private bool player2PlaceShips = false;
        private string player1Name;
        private string player2Name;
        private char[,] player1BattleshipPlayfield = new char[ROW_NUM, COL_NUM];
        private Grid player1PlayfieldGrid;

        public ShipPlacement(string player1Name)
        {
            InitializeComponent();

            vsComputer = true;
            this.player1Name = player1Name;

            SetTitlesForPlayer(player1Name);
        }

        public ShipPlacement(string player1Name, string player2Name)
        {
            InitializeComponent();

            vsComputer = false;
            this.player1Name = player1Name;
            this.player2Name = player2Name;

            SetTitlesForPlayer(player1Name);
        }

        public ShipPlacement(string player1Name, string player2Name, Grid playfield, char[,] battleshipPlayfield)
        {
            InitializeComponent();

            player2PlaceShips = true;
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            this.player1BattleshipPlayfield = battleshipPlayfield;
            this.player1PlayfieldGrid = playfield;

            SetTitlesForPlayer(player2Name);
        }

        private void SetTitlesForPlayer(String playername)
        {
            this.Title = playername + "'s ship placement";
            welcomeLabel.Content = playername + "'s ship placement";
        }

        private void OnGridMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                int shipLength = ShipLengthCalculate();
                DeleteShipShadow(shipLength);
                shipShadow = false;

                bool shipPlacementEnoughSpace = true;

                for (int i = 0; i < shipLength; i++)
                {
                    int cell = CalculateCell();

                    Rectangle ship = ShipSettings();

                    shipPlacementEnoughSpace = !ShipExtendsBeyond(cell, shipLength, shipHorizontal);

                    if (shipPlacementEnoughSpace)
                    {
                        shipPlacementEnoughSpace = !ShipCollision(i, cell, shipLength, shipHorizontal);
                    }

                    if (shipPlacementEnoughSpace)
                    {
                        ShipPlaceToPlayfield(ship, i, cell, shipHorizontal);
                    }
                    else
                    {
                        break;
                    }
                }

                if (shipPlacementEnoughSpace)
                {
                    SetSelectedShipButtonDisabled();
                }
            }
        }

        private void SetSelectedShipButtonDisabled()
        {
            switch (selectedShip)
            {
                case "Carrier":
                    carrierBtn.IsEnabled = false;
                    break;

                case "Battleship":
                    battleshipBtn.IsEnabled = false;
                    break;

                case "Cruiser":
                    cruiserBtn.IsEnabled = false;
                    break;

                case "Submarine":
                    submarineBtn.IsEnabled = false;
                    break;

                case "Destroyer":
                    destroyerBtn.IsEnabled = false;
                    break;
            }

            selectedShip = null;
        }

        private void ShipPlaceToPlayfield(Rectangle ship, int i, int cell, bool shipHorizontal)
        {
            if (shipHorizontal)
            {
                Grid.SetRow(ship, cell / ROW_NUM);
                Grid.SetColumn(ship, cell % COL_NUM + i);

                //save the ship position
                battleshipPlayfield[cell / ROW_NUM, cell % COL_NUM + i] = selectedShipUnit;
            }
            else if (!shipHorizontal)
            {
                Grid.SetRow(ship, cell / ROW_NUM + i);
                Grid.SetColumn(ship, cell % COL_NUM);

                //save the ship position
                battleshipPlayfield[cell / ROW_NUM + i, cell % COL_NUM] = selectedShipUnit;
            }

            playfield.Children.Add(ship);
        }

        private bool ShipExtendsBeyond(int cell, int shipLength, bool shipHorizontal)
        {
            if (shipHorizontal)
            {
                if (cell / ROW_NUM < ROW_NUM && cell % COL_NUM + shipLength - 1 < COL_NUM)
                {
                    return false;
                }
            }
            else
            {
                if (cell / ROW_NUM + shipLength - 1 < ROW_NUM && cell % COL_NUM < COL_NUM)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ShipCollision(int i, int cell, int shipLength, bool shipHorizontal)
        {
            for (int unit = 0 + i; unit < shipLength; unit++)
            {
                if (shipHorizontal)
                {
                    if (char.IsDigit(battleshipPlayfield[cell / ROW_NUM, cell % COL_NUM + unit]))
                    {
                        return true;
                    }
                }
                else if (!shipHorizontal)
                {
                    if (char.IsDigit(battleshipPlayfield[cell / ROW_NUM + unit, cell % COL_NUM]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Rectangle ShipSettings()
        {
            Rectangle ship = new()
            {
                Fill = shipFillBrush
            };
            var Y = playfield.Width / ROW_NUM;
            var X = playfield.Height / COL_NUM;
            ship.Width = Y;
            ship.Height = X;

            ship.Name = selectedShip;

            return ship;
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e) //ship shadow
        {
            int shipLength = ShipLengthCalculate();

            if (shipLength != 0)
            {
                int cell = CalculateCell();

                if (calculatedCell != cell)
                {
                    calculatedCell = cell;

                    DeleteShipShadow(shipLength);

                    for (int i = 0; i < shipLength; i++)
                    {
                        Rectangle shadow = ShadowUnitSettings();

                        // horizontal/vertical ship alignment
                        if (!shipHorizontal)
                        {
                            Grid.SetRow(shadow, cell / ROW_NUM + i);
                            Grid.SetColumn(shadow, cell % COL_NUM);
                        }
                        else if (shipHorizontal)
                        {
                            Grid.SetRow(shadow, cell / ROW_NUM);
                            Grid.SetColumn(shadow, cell % COL_NUM + i);
                        }

                        shipShadow = true;
                        playfield.Children.Add(shadow);
                    }
                }
            }
        }

        private Rectangle ShadowUnitSettings()
        {
            Rectangle shadow = new()
            {
                Fill = shadowFillBrush
            };
            var Y = playfield.Width / ROW_NUM;
            var X = playfield.Height / COL_NUM;
            shadow.Width = Y;
            shadow.Height = X;

            return shadow;
        }

        private int CalculateCell()
        {
            var point = Mouse.GetPosition(playfield);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            foreach (var rowDefinition in playfield.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            foreach (var columnDefinition in playfield.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            return (row * 10) + col;
        }

        private int ShipLengthCalculate()
        {
            int length = selectedShip switch
            {
                "Destroyer" => 1,
                "Submarine" => 2,
                "Cruiser" => 3,
                "Battleship" => 4,
                "Carrier" => 5,
                _ => 0,
            };
            return length;
        }

        private void DeleteShipShadow(int shipLength)
        {
            if (shipShadow == true)
            {
                for (int i = 0; i < shipLength; i++)
                {
                    int lastItem = playfield.Children.Count - 1;
                    playfield.Children.RemoveAt(lastItem);
                }
            }
        }

        private bool EveryShipPlaced()
        {
            if (!destroyerBtn.IsEnabled && !submarineBtn.IsEnabled && !cruiserBtn.IsEnabled && !battleshipBtn.IsEnabled && !carrierBtn.IsEnabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
            playfield.Children.Clear();

            battleshipPlayfield = new char[ROW_NUM, COL_NUM];

            carrierBtn.IsEnabled = false;
            battleshipBtn.IsEnabled = false;
            cruiserBtn.IsEnabled = false;
            submarineBtn.IsEnabled = false;
            destroyerBtn.IsEnabled = false;

            Random rnd = new();

            int randomOrient;

            for (int i = 5; i > 0; i--)
            {
                bool empty = false;
                int randomPosX;
                int randomPosY;
                randomOrient = rnd.Next(0, 2);
                switch (randomOrient)
                {
                    case 0:
                        {
                            //horizontal generating
                            randomPosX = rnd.Next(0, 10 - i + 1);
                            randomPosY = rnd.Next(0, 10);

                            while (empty == false)
                            {
                                if ((randomPosX != 0 && char.IsDigit(battleshipPlayfield[randomPosY, randomPosX - 1])) || ((randomPosX + i - 1) != 9 && char.IsDigit(battleshipPlayfield[randomPosY, randomPosX + i])))
                                {
                                    randomPosX = rnd.Next(0, 10 - i + 1);
                                    randomPosY = rnd.Next(0, 10);
                                }
                                else
                                {
                                    for (int k = 0; k < i; k++)
                                    {
                                        if (char.IsDigit(battleshipPlayfield[randomPosY, randomPosX + k]) || (randomPosY != 0 && char.IsDigit(battleshipPlayfield[randomPosY - 1, randomPosX + k])) || (randomPosY != 9 && char.IsDigit(battleshipPlayfield[randomPosY + 1, randomPosX + k])))
                                        {
                                            randomPosX = rnd.Next(0, 10 - i + 1);
                                            randomPosY = rnd.Next(0, 10);
                                            break;
                                        }
                                        else if (k == (i - 1))
                                        {
                                            empty = true;
                                        }
                                    }
                                }
                            }

                            for (int col = 0; col < i; col++)
                            {
                                Rectangle ship = ShipSettings(i);

                                Grid.SetRow(ship, randomPosY);
                                Grid.SetColumn(ship, col + randomPosX);

                                battleshipPlayfield[randomPosY, randomPosX + col] = Convert.ToChar(i.ToString());
                                playfield.Children.Add(ship);
                            }
                            break;
                        }
                    case 1:
                        {
                            //vertical generating
                            randomPosY = rnd.Next(0, 10 - i + 1);
                            randomPosX = rnd.Next(0, 10);

                            while (empty == false)
                            {
                                if ((randomPosY != 0 && char.IsDigit(battleshipPlayfield[randomPosY - 1, randomPosX])) || ((randomPosY + i - 1) != 9 && char.IsDigit(battleshipPlayfield[randomPosY + i, randomPosX])))
                                {
                                    randomPosY = rnd.Next(0, 10 - i + 1);
                                    randomPosX = rnd.Next(0, 10);
                                }
                                else
                                {
                                    for (int k = 0; k < i; k++)
                                    {
                                        if (char.IsDigit(battleshipPlayfield[randomPosY + k, randomPosX]) || (randomPosX != 0 && char.IsDigit(battleshipPlayfield[randomPosY + k, randomPosX - 1])) || (randomPosX != 9 && char.IsDigit(battleshipPlayfield[randomPosY + k, randomPosX + 1])))
                                        {
                                            randomPosY = rnd.Next(0, 10 - i + 1);
                                            randomPosX = rnd.Next(0, 10);
                                            break;
                                        }
                                        else if (k == (i - 1))
                                        {
                                            empty = true;
                                        }
                                    }
                                }
                            }

                            for (int row = 0; row < i; row++)
                            {
                                Rectangle ship = ShipSettings(i);

                                Grid.SetRow(ship, row + randomPosY);
                                Grid.SetColumn(ship, randomPosX);

                                battleshipPlayfield[randomPosY + row, randomPosX] = Convert.ToChar(i.ToString());
                                playfield.Children.Add(ship);
                            }

                            break;
                        }
                }
            }
        }

        private Rectangle ShipSettings(int shipLength)
        {
            Rectangle ship = new()
            {
                Fill = shipFillBrush
            };
            double Y = playfield.Width / ROW_NUM;
            double X = playfield.Height / COL_NUM;
            ship.Width = Y;
            ship.Height = X;

            ShipSetName(ship, shipLength);

            return ship;
        }

        private void ShipSetName(Rectangle ship, int shipLength)
        {
            switch (shipLength)
            {
                case 1:
                    ship.Name = "Destroyer";
                    break;

                case 2:
                    ship.Name = "Submarine";
                    break;

                case 3:
                    ship.Name = "Cruiser";
                    break;

                case 4:
                    ship.Name = "Battleship";
                    break;

                case 5:
                    ship.Name = "Carrier";
                    break;
            }
        }

        private void ShipBtn(object sender, RoutedEventArgs e)
        {
            if(selectedShip == null)
            {
                Button ShipButton = (Button)sender;
                selectedShip = ShipButton.Content.ToString();

                switch (selectedShip)
                {
                    case "Destroyer":
                        selectedShipUnit = '1';
                        break;

                    case "Submarine":
                        selectedShipUnit = '2';
                        break;

                    case "Cruiser":
                        selectedShipUnit = '3';
                        break;

                    case "Battleship":
                        selectedShipUnit = '4';
                        break;

                    case "Carrier":
                        selectedShipUnit = '5';
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please, put down the current ship before selecting an another ship.", "Warning: ship selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }

        private void RotateBtn_Click(object sender, RoutedEventArgs e)
        {
            shipHorizontal = !shipHorizontal;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new();
            Close();
            mainWindow.Show();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            selectedShip = null;
            calculatedCell = -1;
            shipShadow = false;

            carrierBtn.IsEnabled = true;
            battleshipBtn.IsEnabled = true;
            cruiserBtn.IsEnabled = true;
            submarineBtn.IsEnabled = true;
            destroyerBtn.IsEnabled = true;

            playfield.Children.Clear();

            battleshipPlayfield = new char[ROW_NUM, COL_NUM];
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EveryShipPlaced())
            {
                if (player2PlaceShips)
                {
                    //player vs player game start
                    PvPGameWindow game2PlayerWindow = new(player1Name, player1PlayfieldGrid, player1BattleshipPlayfield, player2Name, playfield, battleshipPlayfield);
                    Close();
                    game2PlayerWindow.Show();
                }
                else if (vsComputer)
                {
                    //player vs computer game start
                    GameWindow gameWindow = new(player1Name, playfield, battleshipPlayfield);
                    Close();
                    gameWindow.Show();
                }
                else if (!vsComputer)
                {
                    //second player's ship placement
                    ShipPlacement player2ShipPlacementWindow = new(player1Name, player2Name, playfield, battleshipPlayfield);
                    Close();
                    player2ShipPlacementWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("All ships must be placed before submitting!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}