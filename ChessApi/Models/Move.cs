using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChessApi.Models {
    public class Move {
        [Key]
        public int Id { get; set; }

        // Standard algebraic notation
        public string SAN { get; set; }

        public Game Game { get; set; }
        
        public int GameId { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
