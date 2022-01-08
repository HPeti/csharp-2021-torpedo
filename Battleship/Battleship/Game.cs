using System.ComponentModel.DataAnnotations;

namespace Battleship
{
    /// <summary>
    /// A class represents a Battleship game. Used for creating a Database schema. 
    /// </summary>
    public class Game
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Player1 { get; set; }
        [Required]
        public string Player2 { get; set; }
        public int Rounds { get; set; }
        public int Player1Hits { get; set; }
        public int Player2Hits { get; set; }
        [Required]
        public string Winner { get; set; }
    }
}
