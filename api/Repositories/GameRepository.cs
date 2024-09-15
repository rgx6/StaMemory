using Microsoft.EntityFrameworkCore;
using StaMemory.Database;

namespace StaMemory.Repositories;

public interface IGameRepository
{
    Task<Game?> GetPlayingGameAsync(string playerId, int currentSeasonId);
    Task RegisterGameAsync(Game game);

    Task<IList<Game>> GetGameListAsync(int seasonId);
}

public class GameRepository : IGameRepository
{
    private readonly StaMemoryDbContext _dbContext;

    public GameRepository(StaMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Game?> GetPlayingGameAsync(string playerId, int currentSeasonId)
    {
        return await _dbContext.Games
            .FirstOrDefaultAsync(x =>
                x.PlayerId == playerId &&
                x.SeasonId == currentSeasonId &&
                x.Status == Constants.GameStatus.Playing);
    }

    public async Task RegisterGameAsync(Game game)
    {
        await _dbContext.Games.AddAsync(game);
    }

    public async Task<IList<Game>> GetGameListAsync(int seasonId)
    {
        return await _dbContext.Games
            .Where(x => x.SeasonId == seasonId && x.Status == Constants.GameStatus.Clear)
            .ToListAsync();
    }
}
