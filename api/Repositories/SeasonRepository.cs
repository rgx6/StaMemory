using Microsoft.EntityFrameworkCore;
using StaMemory.Database;

namespace StaMemory.Repositories;

public interface ISeasonRepository
{
    Task<IList<Season>> GetSeasonListAsync();
    Task<Season?> GetCurrentSeasonAsync();
}

public class SeasonRepository : ISeasonRepository
{
    private readonly StaMemoryDbContext _dbContext;

    public SeasonRepository(StaMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Season>> GetSeasonListAsync()
    {
        return await _dbContext.Seasons
            .OrderBy(x => x.IsCurrentSeason ? 1 : 2)
                .ThenByDescending(x => x.SeasonId)
            .ToListAsync();
    }

    public async Task<Season?> GetCurrentSeasonAsync()
    {
        return await _dbContext.Seasons
            .SingleOrDefaultAsync(x => x.IsCurrentSeason);
    }
}
