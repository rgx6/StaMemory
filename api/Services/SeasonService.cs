using StaMemory.Database;
using StaMemory.Repositories;

namespace StaMemory.Services;

public interface ISeasonService
{
    Task<IList<Season>> GetSeasonListAsync();
}

public class SeasonService : ISeasonService
{
    private readonly IUnitOfWork _unitOfWork;

    public SeasonService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IList<Season>> GetSeasonListAsync()
    {
        return await _unitOfWork.SeasonRepository.GetSeasonListAsync();
    }
}
