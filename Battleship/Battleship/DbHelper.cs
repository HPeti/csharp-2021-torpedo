﻿namespace Battleship
{
    public class DbHelper
    {
        public static void InsertToDb(string player1, string player2, int rounds, int player1Hits, int player2Hits)
        {
            using GameDbContext database = new();
            database.Games.Add(new Game { Player1 = player1, Player2 = player2, Rounds = rounds, Player1Hits = player1Hits, Player2Hits = player2Hits });

            database.SaveChanges();
        }
    }
}
