using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// A class for AI helper methods.
    /// </summary>
    public static class AI
    {
        /// <summary>
        /// Determines whether the cell was shooted previously or not.
        /// </summary>
        /// <param name="x">X coordiante</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="table">The player's table represented as a 2D char array.</param>
        /// <returns>Returns true if the cell contains 'S' (shooted) or 'H' (hit) characters.</returns>
        public static bool IsShootedCell(int x, int y, char[,] table)
        {
            return table[x, y] is 'S' or 'H';
        }

        /// <summary>
        /// Generate a random X and Y coordinate which represents an AI shoot.
        /// </summary>
        /// <param name="rnd">Random</param>
        /// <param name="table">The player's table represented as a 2D char array.</param>
        /// <returns>The index which represents the AI shoot.</returns>
        public static int GenerateShoot(Random rnd, char[,] table)
        {
            int rndX;
            int rndY;

            do
            {
                rndX = rnd.Next(0, 10);
                rndY = rnd.Next(0, 10);
            }
            while (IsShootedCell(rndX, rndY, table));

            return (rndY * 10) + rndX;
        }

        /// <summary>
        /// Determines whether the cell is occupied by one of the player's units.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="table"></param>
        /// <returns>Returns true if the cell contains numeric character (the player's ships represented as a number in the array).</returns>
        public static bool IsPlayerUnit(int x, int y, char[,] table)
        {
            return char.IsDigit(table[x, y]);
        }

        /// <summary>
        /// Determines whether the cell (given by the coordinates) is outside of the border of the player's table.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Returns true, if the cell is outside of the border of the player's table..</returns>
        public static bool DetectBorder(int x, int y)
        {
            return x is < 0 or > 9 || y is < 0 or > 9;
        }

        /*private Rectangle CreateShip(int shipSize)
        {
            Rectangle ship = new()
            {
                Fill = Brushes.AliceBlue
            };

            double x = rightTable.Height / 10;
            double y = rightTable.Width / 10;

            ship.Height = x;
            ship.Width = y;
            //ship.Visibility = Visibility.Hidden;

            return ship;
        }

        private void GenerateAItable(Random rnd, )
        {
            int orient;
            int randomX;
            int randomY;
            bool empty;

            for (int i = 5; i > 0; i--)
            {
                empty = false;
                orient = rnd.Next(0, 2);

                if (orient == 0) //Függőleges
                {
                    randomX = rnd.Next(0, 10);
                    randomY = rnd.Next(0, 10 - i + 1);

                    while (empty == false)
                    {
                        if ((randomY != 0 && char.IsDigit(_aiTable[randomY - 1, randomX])) || (randomY + i - 1 != 9 && char.IsDigit(_aiTable[randomY + i, randomX])))
                        {
                            randomX = rnd.Next(0, 10);
                            randomY = rnd.Next(0, 10 - i + 1);
                        }
                        else
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (char.IsDigit(_aiTable[randomY + j, randomX]) || (randomX != 0 && char.IsDigit(_aiTable[randomY + j, randomX - 1])) || (randomX != 9 && char.IsDigit(_aiTable[randomY + j, randomX + 1])))
                                {
                                    randomX = rnd.Next(0, 10);
                                    randomY = rnd.Next(0, 10 - i + 1);

                                    break;
                                }
                                else if (j == (i - 1))
                                {
                                    empty = true;
                                }
                            }
                        }
                    }

                    for (int row = 0; row < i; row++)
                    {
                        Rectangle ship = CreateShip(i);

                        Grid.SetColumn(ship, row + randomY);
                        Grid.SetRow(ship, randomX);

                        _aiTable[randomY + row, randomX] = Convert.ToChar(i.ToString());
                        rightTable.Children.Add(ship);
                    }
                }
                else //Vízszintes
                {
                    randomX = rnd.Next(0, 10);
                    randomY = rnd.Next(0, 10 - i + 1);
                }
            }
        }*/
    }
}
