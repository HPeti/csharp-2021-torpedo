using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        private int playerChangeCounter = 0;

        private string player1Name;
        private string player2Name;
        private int rounds = 1;
        private int player1Hits = 0;
        private int player2Hits = 0;

        PvPGameWindow player2Window;
        PvPGameWindow player1Window;
        Random rnd = new();

        public delegate string Hit(int cell); 
        public event Hit OnHit;

        public delegate void CloseWindow();
        public event CloseWindow onCloseWindow;

        public PvPGameWindow(string player1Name, Grid player1PlayfieldGrid, char[,] player1Playfield, string player2Name, Grid player2PlayfieldGrid, char[,] player2Playfield)
        {
            InitializeComponent();
            player1Window = this;
            this.Title = player1Name;
            this.myPlayfield = player1Playfield;

            this.player1Name = player1Name;
            this.player2Name = player2Name;
            windowPlayer1 = true;
            string playerStart = WhichPlayerStart(player1Name, player2Name);

            player2Window = new PvPGameWindow(player1Name, player2Name, player2PlayfieldGrid, player2Playfield, player1Coming, player1Window);
            player2Window.Title = player2Name;
            player2Window.Show();
            player1Window.Left = 0;
            player1Window.Top = 0;
            player2Window.Left = 0;
            player2Window.Top = 0;
            if (player1Coming)
            {
                player1Window.WindowState = WindowState.Normal;
                player2Window.WindowState = WindowState.Minimized;
            }
            else
            {
                player2Window.WindowState = WindowState.Normal;
                player1Window.WindowState = WindowState.Minimized;
            }

            InitializeLabels(player1Name, player2Name, playerStart);

            SharedUtility.ShipStatHpInit(carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);
            PlayerShipsLoad(player1PlayfieldGrid);

            player2Window.OnHit += new Hit(this.OnShoot);
            this.OnHit += new Hit(player2Window.OnShoot);

            player2Window.onCloseWindow += new CloseWindow(this.OnClose);
            this.onCloseWindow += new CloseWindow(player2Window.OnClose);

        }

        public PvPGameWindow(string player1Name, string player2Name, Grid player2PlayfieldGrid, char[,] player2Playfield, bool player1Coming, PvPGameWindow player1Window)
        {
            InitializeComponent();
            this.player1Window = player1Window;
            player2Window = this;
            windowPlayer1 = false;
            this.myPlayfield = player2Playfield;
            this.player1Coming = player1Coming;
            this.player1Name = player1Name;
            this.player2Name = player2Name;
            

            SharedUtility.ShipStatHpInit(carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);
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
            
            SharedUtility.RoundsLabelChange(roundsLabel,ref playerChangeCounter,ref rounds);
            WhichPlayerComingLabelChange();

            return "false";
        }

        private bool IsHitShipUnit(int cell)
        {
            return char.IsDigit(myPlayfield[cell / rows, cell % columns]);
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

        private void OnGridMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                if ((player1Coming && windowPlayer1) || (!player1Coming && !windowPlayer1))
                {
                    DeleteShadow();
                    shadowExists = false;

                    int cell = SharedUtility.CalculateCell(rightTable);

                    bool shooted = IsCellShooted(cell);

                    if (!shooted)
                    {
                        string shipUnitName = OnHit(cell);

                        if (shipUnitName != "false")
                        {
                            SetShipUnit(cell, true, false);

                            SharedUtility.ShipHpDecrement(shipUnitName, carrierHpGrid, battleshipHpGrid, cruiserHpGrid, submarineHpGrid, destroyerHpGrid);
                            enemyPlayfield[cell / rows, cell % columns] = 'T';

                            HitsLabelChange();

                            if (player1Hits == 15)
                            {
                                SharedUtility.EndGame(player1Name, player2Name, rounds, player1Hits, player2Hits, player1Name);
                            }
                            else if (player2Hits == 15)
                            {
                                SharedUtility.EndGame(player1Name, player2Name, rounds, player1Hits, player2Hits, player2Name);
                            }
                        }
                        else
                        {
                            SetShipUnit(cell, false, false);
                            enemyPlayfield[cell / rows, cell % columns] = 'V';

                            player1Coming = !player1Coming;
                            SharedUtility.RoundsLabelChange(roundsLabel, ref playerChangeCounter, ref rounds);
                            WhichPlayerComingLabelChange();
                            if (player1Coming)
                            {
                                Thread.Sleep(500);
                                player1Window.WindowState = WindowState.Normal;
                                player2Window.WindowState = WindowState.Minimized;
                            }
                            else
                            {
                                Thread.Sleep(500);
                                player2Window.WindowState = WindowState.Normal;
                                player1Window.WindowState = WindowState.Minimized;
                            }
                        }
                    }
                }
            }
        }


        private void HitsLabelChange()
        {
            if (windowPlayer1 && player1Coming)
            {
                player1Hits++;
                player1HitsLabel.Content = player1Hits;
                //player1HitsLabel.Content = Convert.ToInt32(player1HitsLabel.Content) + 1;
            }
            else if (!windowPlayer1 && !player1Coming)
            {
                player2Hits++;
                player2HitsLabel.Content = player2Hits++;
                //player2HitsLabel.Content = Convert.ToInt32(player2HitsLabel.Content) + 1;
            }

            if (windowPlayer1 && !player1Coming)
            {
                player2Hits++;
                player2HitsLabel.Content = player2Hits++;
                //player2HitsLabel.Content = Convert.ToInt32(player2HitsLabel.Content) + 1;
            }
            else if (!windowPlayer1 && player1Coming)
            {
                player1Hits++;
                player1HitsLabel.Content = player1Hits;
                //player1HitsLabel.Content = Convert.ToInt32(player1HitsLabel.Content) + 1;
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
            return enemyPlayfield[cell / rows, cell % columns] is 'H' or 'M';
        }

        private void OnGridMouseOver(object sender, MouseEventArgs e) //ship shadow
        {
            int cell = SharedUtility.CalculateCell(rightTable);

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

        public void OnClose()
        {
            this.Close();
        }

        private void SurrendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (windowPlayer1)
            {
                DbHelper.InsertToDb(player1Name, player2Name, rounds, player1Hits, player2Hits, player2Name);

                MainWindow main = new();
                Close();
                player2Window.Close();
                main.Show();
            }
            else
            {
                DbHelper.InsertToDb(player1Name, player2Name, rounds, player1Hits, player2Hits, player1Name);

                this.onCloseWindow();
                MainWindow main = new();
                Close();
                
                main.Show();
            }
        }

    }
}