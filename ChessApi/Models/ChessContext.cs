using Microsoft.EntityFrameworkCore;
namespace ChessApi.Models {
    public class ChessContext: DbContext {
        public string ConnectionString { get; set; }

        public ChessContext(DbContextOptions<ChessContext> options) : base(options) { }

        public DbSet<Game> Game{ get; set; }
        public DbSet<Move> Move { get; set; }

    }
}
