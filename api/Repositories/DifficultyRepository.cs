using Microsoft.EntityFrameworkCore;
using StaMemory.Database;

namespace StaMemory.Repositories;

public interface IDifficultyRepository
{
    Task<IList<Difficulty>> GetDifficultyListAsync();
    Task<Difficulty?> GetDifficultyAsync(int difficultyId);
}

public class DifficultyRepository : IDifficultyRepository
{
    private readonly StaMemoryDbContext _dbContext;

    public DifficultyRepository(StaMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<Difficulty>> GetDifficultyListAsync()
    {
        return await _dbContext.Difficulty
            .OrderBy(x => x.DifficultyId)
            .ToListAsync();
    }

    public async Task<Difficulty?> GetDifficultyAsync(int difficultyId)
    {
        return await _dbContext.Difficulty
            .SingleOrDefaultAsync(x => x.DifficultyId == difficultyId);
    }
}
