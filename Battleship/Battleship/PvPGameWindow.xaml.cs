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
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for PvPGameWindow.xaml
    /// </summary>
    public partial class PvPGameWindow : Window
    {
        private int calculatedCell = -1;
        private const int rows = 10;
        private const int columns = 10;
        private bool shadowExists = false;
        private char[,] myPlayfield = new char[10, 10];
        private char[,] enemyPlayfield = new char[10, 10];
        private bool player1Coming;
        private bool windowPlayer1;

        private int changePlayerCounter = 0;

        private string player1Name;
        private string player2Name;

        PvPGameWindow player2Window;
        Random rnd = new Random();

        public delegate string Hit(int cell); 
        public event Hit OnHit;

        public delegate void CloseWindow();
        public event CloseWindow onCloseWindow;

        public PvPGameWindow(string player1Name, Grid player1PlayfieldGrid, char[,] player1Playfield, string player2Name, Grid player2PlayfieldGrid, char[,] player2Playfield)
        {
            InitializeComponent();
            this.Title = player1Name;
            this.myPlayfield = player1Playfield;

            this.player1Name = player1Name;
            this.player2Name = player2Name;
            windowPlayer1 = true;
            string playerStart = WhichPlayerStart(player1Name, player2Name);

            player2Window = new PvPGameWindow(player1Name, player2Name, player2PlayfieldGrid, player2Playfield, player1Coming);
            player2Window.Title = player2Name;
            player2Window.Show();

            InitializeLabels(player1Name, player2Name, playerStart);

            ShipStatHpInit();
            PlayerShipsLoad(player1PlayfieldGrid);

            player2Window.OnHit += new Hit(this.OnShoot);
            this.OnHit += new Hit(player2Window.OnShoot);

            player2Window.onCloseWindow += new CloseWindow(this.OnClose);
            this.onCloseWindow += new CloseWindow(player2Window.OnClose);

        }

        public PvPGameWindow(string player1Name, string player2Name, Grid player2PlayfieldGrid, char[,] player2Playfield, bool player1Coming)
        {
            InitializeComponent();

            windowPlayer1 = false;
            this.myPlayfield = player2Playfield;
            this.player1Coming = player1Coming;
            this.player1Name = player1Name;
            this.player2Name = player2Name;

            ShipStatHpInit();
            PlayerShipsLoad(player2PlayfieldGrid);
        }

        private string WhichPlayerStart(string player1Name, string player2Name)
        {
            if (rnd.Next(0, 2) == 0)
            {
                player1Coming = true;
                return player1Name;
            }
            else
            {
                player1Coming = false;
                return player2Name;
            }
        }

        private void InitializeLabels(string player1Name, string player2Name, string playerStart)
        {
            player1NameLabel.Content = player1Name + " Hits:";
            player2NameLabel.Content = player2Name + " Hits:";

            player2Window.player1NameLabel.Content = player1Name + " Hits:";
            player2Window.player2NameLabel.Content = player2Name + " Hits:";

            playerComingLabel.Content = playerStart + " is coming";
            player2Window.playerComingLabel.Content = playerStart + " is coming";
        }

        private void PlayerShipsLoad(Grid playfield)
        {
            for (int unit = playfield.Children.Count - 1; unit >= 0; unit--)
            {
                var child = playfield.Children[unit];
                playfield.Children.RemoveAt(unit);
                leftTable.Children.Add(child);
            }
        }

        private void ShipStatHpInit()
        {
            for (int ship = 5; ship > 0; ship--)
            {
                for (int unit = 0; unit < ship; unit++)
                {
                    Rectangle hpUnit = ShipHpSettings(ship);

                    Grid.SetColumn(hpUnit, unit);

                    switch (ship)
                    {
                        case 5:
                            carrierHpGrid.Children.Add(hpUnit);
                            break;
                        case 4:
                            battleshipHpGrid.Children.Add(hpUnit);
                            break;
                        case 3:
                            cruiserHpGrid.Children.Add(hpUnit);
                            break;
                        case 2:
                            submarineHpGrid.Children.Add(hpUnit);
                            break;
                        case 1:
                            destroyerHpGrid.Children.Add(hpUnit);
                            break;
                    }
                }
            }
        }

        public string OnShoot(int cell)
        {
            bool isHit = IsHitShipUnit(cell);

            SetShipUnit(cell, isHit, true);

            if (isHit)
            {
                HitsLabelChange();
                return myPlayfield[cell / rows, cell % columns].ToString();
            }

            player1Coming = !player1Coming;
            RoundsLabelChange();
            WhichPlayerComingLabelChange();

            return "false";
        }

        private bool IsHitShipUnit(int cell)
        {
            if (char.IsDigit(myPlayfield[cell / rows, cell % columns]))
            {
                return true;
            }

            return false;
        }

        private void SetShipUnit(int cell, bool isHit, bool setLeftTable)
        {
            Rectangle ship = ShipUnitSettings(isHit);

            Grid.SetRow(ship, cell / rows);
            Grid.SetColumn(ship, cell % columns);

            if (setLeftTable)
            {
                leftTable.Children.Add(ship);
            }
            else
            {
                rightTable.Children.Add(ship);
            }
        }

        private Rectangle ShipUnitSettings(bool isHit)
        {
            Rectangle unit = new Rectangle();

            if (isHit)
            {
                unit.Fill = Brushes.DarkRed;
            }
            else
            {
                unit.Fill = Brushes.LightGray;
            }

            var Y = unit.Width / rows;
            var X = unit.Height / columns;
            unit.Width = Y;
            unit.Height = X;

            return unit;
        }

        private Rectangle ShipHpSettings(int shipLength)
        {
            Rectangle hpUnit = new Rectangle();
            hpUnit.Fill = Brushes.Green;
            var Y = carrierHpGrid.Width;
            var X = carrierHpGrid.Height / shipLength;
            hpUnit.Width = Y;
            hpUnit.Height = X;
            return hpUnit;
        }

        private void OnGridMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                if (player1Coming && windowPlayer1 || !player1Coming && !windowPlayer1)
                {
                    DeleteShadow();
                    shadowExists = false;

                    int cell = CalculateCell();

                    bool shooted = IsCellShooted(cell);

                    if (!shooted)
                    {
                        string shipUnitName = this.OnHit(cell);

                        if (shipUnitName != "false")
                        {
                            SetShipUnit(cell, true, false);
                            ShipHpDecrement(shipUnitName);
                            enemyPlayfield[cell / rows, cell % columns] = 'T';

                            HitsLabelChange();
                            EveryShipDestroyed();
                        }
                        else
                        {
                            SetShipUnit(cell, false, false);
                            enemyPlayfield[cell / rows, cell % columns] = 'V';

                            player1Coming = !player1Coming;
                            RoundsLabelChange();
                            WhichPlayerComingLabelChange();
                        }
                    }
                }
            }
        }

        private void EveryShipDestroyed()
        {
            if (player1HitsLabel.Content.ToString() == "15")
            {
                MessageBox.Show(player1Name + " won the game!", "The game is over", MessageBoxButton.OK);
                GameEnd(player1Name);
            }
            else if (player2HitsLabel.Content.ToString() == "15")
            {
                MessageBox.Show(player2Name + " won the game!", "The game is over", MessageBoxButton.OK);
                GameEnd(player2Name);
            }
        }

        private void RoundsLabelChange()
        {
            changePlayerCounter++;

            if (changePlayerCounter % 2 == 0)
            {
                roundsLabel.Content = Convert.ToInt32(roundsLabel.Content) + 1;
            }
        }

        private void HitsLabelChange()
        {
            if (windowPlayer1 && player1Coming)
            {
                player1HitsLabel.Content = Convert.ToInt32(player1HitsLabel.Content) + 1;
            }
            else if (!windowPlayer1 && !player1Coming)
            {
                player2HitsLabel.Content = Convert.ToInt32(player2HitsLabel.Content) + 1;
            }

            if (windowPlayer1 && !player1Coming)
            {
                player2HitsLabel.Content = Convert.ToInt32(player2HitsLabel.Content) + 1;
            }
            else if (!windowPlayer1 && player1Coming)
            {
                player1HitsLabel.Content = Convert.ToInt32(player1HitsLabel.Content) + 1;
            }
        }

        private void WhichPlayerComingLabelChange()
        {
            if (player1Coming)
            {
                playerComingLabel.Content = player1Name + " is coming";
            }
            else
            {
                playerComingLabel.Content = player2Name + " is coming";
            }
        }

        private bool IsCellShooted(int cell)
        {
            if (enemyPlayfield[cell / rows, cell % columns] == 'T' || enemyPlayfield[cell / rows, cell % columns] == 'V')
            {
                return true;
            }

            return false;
        }

        private void ShipHpDecrement(string shipUnitName)
        {
            switch (shipUnitName)
            {
                case "5":
                    carrierHpGrid.Children.RemoveAt(carrierHpGrid.Children.Count - 1);
                    break;
                case "4":
                    battleshipHpGrid.Children.RemoveAt(battleshipHpGrid.Children.Count - 1);
                    break;
                case "3":
                    cruiserHpGrid.Children.RemoveAt(cruiserHpGrid.Children.Count - 1);
                    break;
                case "2":
                    submarineHpGrid.Children.RemoveAt(submarineHpGrid.Children.Count - 1);
                    break;
                case "1":
                    destroyerHpGrid.Children.RemoveAt(destroyerHpGrid.Children.Count - 1);
                    break;
            }
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e) //ship shadow
        {
            int cell = CalculateCell();

            if (calculatedCell != cell)
            {
                calculatedCell = cell;

                DeleteShadow();

                Rectangle shadow = ShadowUnitSettings();

                Grid.SetRow(shadow, cell / rows);
                Grid.SetColumn(shadow, cell % columns);

                rightTable.Children.Add(shadow);
                shadowExists = true;
            }
        }

        private Rectangle ShadowUnitSettings()
        {
            var shadow = new Rectangle();
            shadow.Fill = Brushes.LightGray;
            var Y = rightTable.Width / rows;
            var X = rightTable.Height / columns;
            shadow.Width = Y;
            shadow.Height = X;

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

        private int CalculateCell() //which cell the cursor is on
        {
            var point = Mouse.GetPosition(rightTable);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            foreach (var rowDefinition in rightTable.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            foreach (var columnDefinition in rightTable.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            return (row * 10) + col;
        }

        public void OnClose()
        {
            this.Close();
        }

        private void GameEnd(string winner)
        {
           
        }



        private void SurrendBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Stats_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
