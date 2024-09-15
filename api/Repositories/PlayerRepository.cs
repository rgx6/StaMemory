using Microsoft.EntityFrameworkCore;
using StaMemory.Database;

namespace StaMemory.Repositories;

public interface IPlayerRepository
{
    Task<IList<Player>> GetPlayerListAsync();
    Task<Player?> GetPlayerAsync(string playerId);
    Task<string> RegisterPlayerAsync(string playerName);
}

public class PlayerRepository : IPlayerRepository
{
    private readonly StaMemoryDbContext _dbContext;

    public PlayerRepository(StaMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Player>> GetPlayerListAsync()
    {
        return await _dbContext.Players
            .AsNoTracking()
            .Select(x => new Player
            {
                PlayerId = x.PlayerId,
                PlayerName = x.PlayerName,
            })
            .ToListAsync();
    }

    public async Task<Player?> GetPlayerAsync(string playerId)
    {
        var player = await _dbContext.Players
            .SingleOrDefaultAsync(x => x.PlayerId == playerId);

        return player;
    }

    public async Task<string> RegisterPlayerAsync(string playerName)
    {
        var playerId = Utils.IssueId();

        await _dbContext.Players.AddAsync(new Player
        {
            PlayerId = playerId,
            PlayerName = playerName,
            CreatedAt = DateTime.UtcNow,
        });

        return playerId;
    }
}
