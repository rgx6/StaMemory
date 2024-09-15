using StaMemory.Database;
using StaMemory.Repositories;

namespace StaMemory.Services;

public interface IDifficultyService
{
    Task<IList<Difficulty>> GetDifficultyListAsync();
    Task<Difficulty?> GetDifficultyAsync(int difficultyId);
}

public class DifficultyService : IDifficultyService
{
    private readonly IUnitOfWork _unitOfWork;

    public DifficultyService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<Difficulty>> GetDifficultyListAsync()
    {
        return await _unitOfWork.DifficultyRepository.GetDifficultyListAsync();
    }

    public async Task<Difficulty?> GetDifficultyAsync(int difficultyId)
    {
        return await _unitOfWork.DifficultyRepository.GetDifficultyAsync(difficultyId);
    }
}
