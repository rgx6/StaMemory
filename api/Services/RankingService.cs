using StaMemory.ProtocolModels.RankingApi;
using StaMemory.Repositories;

namespace StaMemory.Services;

public interface IRankingService
{
    Task<GetRanking.Response> GetRankingAsync(int seasonId);
}

public class RankingService : IRankingService
{
    private readonly IUnitOfWork _unitOfWork;

    public RankingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRanking.Response> GetRankingAsync(int seasonId)
    {
        var playerList = await _unitOfWork.PlayerRepository.GetPlayerListAsync();

        var gameList = await _unitOfWork.GameRepository.GetGameListAsync(seasonId);

        var groups = gameList.GroupBy(x => x.DifficultyId);

        var result = new GetRanking.Response();

        result.RankingList = groups
            .OrderBy(group => group.Key)
            .Select(group => new GetRanking.Response.Ranking
            {
                DifficultyName = group.First().DifficultyName,
                ScoreList = group
                        .OrderBy(x => x.Turn)
                            .ThenBy(x => x.CompletedAt - x.CreatedAt)
                            .ThenBy(x => x.CompletedAt)
                        .Take(20)
                        .Select(x => new GetRanking.Response.Score
                        {
                            PlayerName = playerList.Single(y => y.PlayerId == x.PlayerId).PlayerName,
                            Turn = x.Turn,
                            ClearTime = Utils.GetClearTime(x.CreatedAt, x.CompletedAt!.Value),
                            ClearedAt = x.CreatedAt.AddHours(9).ToString("yyyy/MM/dd HH:mm"),
                        })
                        .ToList(),
            }).ToList();

        return result;
    }
}
