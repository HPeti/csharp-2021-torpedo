﻿using Microsoft.EntityFrameworkCore;

namespace Battleship
{
    public class GameDbContext : DbContext
    {
        public GameDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = ServerDb");
            }
        }

        public DbSet<Game> Games { get; set; }
    }
}
