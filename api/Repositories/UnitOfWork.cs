using StaMemory.Database;

namespace StaMemory.Repositories;

public interface IUnitOfWork
{
    IDifficultyRepository DifficultyRepository { get; }
    IGameRepository GameRepository { get; }
    IPlayerRepository PlayerRepository { get; }
    ISeasonRepository SeasonRepository { get; }

    Task CommitAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly StaMemoryDbContext _dbContext;

    private IDifficultyRepository? _difficultyRepository;
    private IGameRepository? _gameRepository;
    private IPlayerRepository? _playerRepository;
    private ISeasonRepository? _seasonRepository;

    public UnitOfWork(StaMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IDifficultyRepository DifficultyRepository => _difficultyRepository = _difficultyRepository ?? new DifficultyRepository(_dbContext);
    public IGameRepository GameRepository => _gameRepository = _gameRepository ?? new GameRepository(_dbContext);
    public IPlayerRepository PlayerRepository => _playerRepository = _playerRepository ?? new PlayerRepository(_dbContext);
    public ISeasonRepository SeasonRepository => _seasonRepository = _seasonRepository ?? new SeasonRepository(_dbContext);

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
