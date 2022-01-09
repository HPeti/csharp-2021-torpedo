using System;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Battleship
{
    /// <summary>
    /// A class which contains common methods for a game.
    /// </summary>
    public static class SharedUtility
    {

        private static Random rnd = new ();
        public const int ROWS = 10;
        public const int COLUMNS = 10;
        /// <summary>
        /// Indicates the end of the current game.
        /// </summary>
        /// <param name="player1">Player1's name</param>
        /// <param name="player2">Player2's name</param>
        /// <param name="rounds">Number of rounds</param>
        /// <param name="player1Hits">Player1's hits</param>
        /// <param name="player2Hits">Player2's hits</param>
        /// <param name="winner">The player's name, who won the game</param>
        public static void EndGame(string player1, string player2, int rounds, int player1Hits, int player2Hits, string winner)
        {
            _ = MessageBox.Show("Congratulations! {0} won!", winner);
            DbHelper.InsertToDb(player1, player2, rounds, player1Hits, player2Hits, winner);
        }



        public static string WhichPlayerStart(string player1Name, string player2Name, bool player1Coming)
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

        public static void PlayerShipsLoad(Grid playfield, Grid table)
        {
            for (int unit = playfield.Children.Count - 1; unit >= 0; unit--)
            {
                UIElement child = playfield.Children[unit];
                playfield.Children.RemoveAt(unit);
                table.Children.Add(child);
            }
        }


        public static void ShipStatHpInit(Grid carrierHpGrid, Grid battleshipHpGrid, Grid cruiserHpGrid, Grid submarineHpGrid, Grid destroyerHpGrid)
        {
            for (int ship = 5; ship > 0; ship--)
            {
                for (int unit = 0; unit < ship; unit++)
                {
                    Rectangle hpUnit = ShipHpSettings(ship,carrierHpGrid);

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

        public static Rectangle ShipHpSettings(int shipLength, Grid carrierHpGrid)
        {
            Rectangle hpUnit = new Rectangle();
            hpUnit.Fill = Brushes.Green;
            hpUnit.RadiusX = 2;
            hpUnit.RadiusY = 2;
            double Y = carrierHpGrid.Width;
            double X = carrierHpGrid.Height / shipLength;
            hpUnit.Width = Y;
            hpUnit.Height = X;
            return hpUnit;
        }

        public static Rectangle ShipUnitSettings(bool isHit)
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

            double Y = unit.Width / ROWS;
            double X = unit.Height / COLUMNS;
            unit.Width = Y;
            unit.Height = X;

            return unit;
        }


        public static void SetShipUnit(int cell, bool isHit, bool setLeftTable, Grid leftTable, Grid rightTable)
        {
            Rectangle ship = ShipUnitSettings(isHit);

            Grid.SetRow(ship, cell / ROWS);
            Grid.SetColumn(ship, cell % COLUMNS);

            if (setLeftTable)
            {
                leftTable.Children.Add(ship);
            }
            else
            {
                rightTable.Children.Add(ship);
            }
        }


        public static void EveryShipDestroyed(string player1, string player2, int rounds, int player1Hits, int player2Hits)
        {
            if (player1Hits == 15)
            {
                EndGame(player1, player2, rounds, player1Hits, player2Hits, player1);
            }
            else if (player2Hits == 15)
            {
                EndGame(player1, player2, rounds, player1Hits, player2Hits, player2);
            }
        }


        public static void RoundsLabelChange(Label roundsLabel,ref int playerChangeCounter)
        {
            playerChangeCounter++;

            if (playerChangeCounter % 2 == 0)
            {
                roundsLabel.Content = Convert.ToInt32(roundsLabel.Content) + 1;
            }
        }

        public static bool IsCellShooted(int cell,ref char[,] enemyPlayfield )
        {
            return enemyPlayfield[cell / ROWS, cell % COLUMNS] is 'H' or 'M';
        }

        public static void ShipHpDecrement(string shipUnitName, Grid carrierHpGrid, Grid battleshipHpGrid, Grid cruiserHpGrid, Grid submarineHpGrid, Grid destroyerHpGrid)
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
                default:
                    break;
            }
        }

        public static int CalculateCell(Grid rightTable) //which cell the cursor is on
        {
            Point point = Mouse.GetPosition(rightTable);

            int row = 0;
            int col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

            foreach (RowDefinition rowDefinition in rightTable.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            foreach (ColumnDefinition columnDefinition in rightTable.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            return (row * 10) + col;
        }

    }
}
