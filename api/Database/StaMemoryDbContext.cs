using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace StaMemory.Database;

public class StaMemoryDbContext : DbContext
{
    public DbSet<Difficulty> Difficulty { get; init; }
    public DbSet<Game> Games { get; init; }
    public DbSet<Player> Players { get; init; }
    public DbSet<Season> Seasons { get; init; }

    public static StaMemoryDbContext Create(IMongoDatabase database)
    {
        var options = new DbContextOptionsBuilder<StaMemoryDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options;

        return new StaMemoryDbContext(options);
    }

    public StaMemoryDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Difficulty>().ToCollection("difficulty");
        modelBuilder.Entity<Game>().ToCollection("games");
        modelBuilder.Entity<Player>().ToCollection("players");
        modelBuilder.Entity<Season>().ToCollection("seasons");
    }
}
