using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
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
    }
}
