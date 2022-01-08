using System;

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


    }
}
