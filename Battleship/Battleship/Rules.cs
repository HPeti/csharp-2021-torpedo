using System.Windows;

namespace Battleship
{
    /// <summary>
    /// A class which contains common methods for a game.
    /// </summary>
    public static class Rules
    {


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
    }
}
