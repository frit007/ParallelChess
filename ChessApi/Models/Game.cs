using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Models {
    public class Game {
        private ChessContext context;

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Opponent { get; set; }

        [InverseProperty("Game")]
        public ICollection<Move> Moves { get; set; }
    }
}
