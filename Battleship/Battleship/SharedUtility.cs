﻿using System;
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
        private static Random rnd = new();
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
            _ = MessageBox.Show("Game Over!");
            DbHelper.InsertToDb(player1, player2, rounds, player1Hits, player2Hits, winner);
        }

        public static bool WhichPlayerStart()
        {
            return rnd.Next(0, 2) == 0;
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
            Rectangle hpUnit = new()
            {
                Fill = Brushes.Green,
                RadiusX = 5,
                RadiusY = 5
            };
            double Y = carrierHpGrid.Width;
            double X = carrierHpGrid.Height / shipLength;
            hpUnit.Width = Y;
            hpUnit.Height = X;
            return hpUnit;
        }

        public static void RoundsLabelChange(Label roundsLabel, ref int playerChangeCounter, ref int rounds)
        {
            playerChangeCounter++;

            if (playerChangeCounter % 2 == 0)
            {
                rounds++;
                roundsLabel.Content = rounds;
            }
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
