using StaMemory.Database;
using StaMemory.Repositories;

namespace StaMemory.Services;

public interface IPlayerService
{
    Task<Player?> GetPlayerAsync(string playerId);
    Task<string> RegisterPlayerAsync(string playerName);
    Task RenamePlayerAsync(Player player, string playerName);
}

public class PlayerService : IPlayerService
{
    private readonly IUnitOfWork _unitOfWork;

    public PlayerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Player?> GetPlayerAsync(string playerId)
    {
        return await _unitOfWork.PlayerRepository.GetPlayerAsync(playerId);
    }

    public async Task<string> RegisterPlayerAsync(string playerName)
    {
        var playerId = await _unitOfWork.PlayerRepository.RegisterPlayerAsync(playerName);

        await _unitOfWork.CommitAsync();

        return playerId;
    }

    public async Task RenamePlayerAsync(Player player, string playerName)
    {
        player.PlayerName = playerName;

        await _unitOfWork.CommitAsync();
    }
}
