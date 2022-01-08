namespace Battleship
{
    /// <summary>
    /// A class which consists useful methods for database.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// A method which can insert a new record (Game entity) to the Database.
        /// </summary>
        /// <param name="player1">Player1's name</param>
        /// <param name="player2">Player2's name</param>
        /// <param name="rounds">Number of rounds</param>
        /// <param name="player1Hits">Number of Player1's hit ships</param>
        /// <param name="player2Hits">Number of Player2's hit ships</param>
        /// <param name="winner">The player's name who won the game</param>
        public static void InsertToDb(string player1, string player2, int rounds, int player1Hits, int player2Hits, string winner)
        {
            using GameDbContext database = new();
            database.Games.Add(new Game { Player1 = player1, Player2 = player2, Rounds = rounds, Player1Hits = player1Hits, Player2Hits = player2Hits, Winner = winner });

            database.SaveChanges();
        }

        /// <summary>
        /// A method which clears the datas from the database and refreshes the Scoreboard.
        /// </summary>
        public static void ClearDb()
        {
            using GameDbContext database = new();
            database.Games.RemoveRange(database.Games);

            database.SaveChanges();
        }
    }
}
