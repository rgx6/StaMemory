using Microsoft.AspNetCore.Mvc;
using StaMemory.ProtocolModels.RankingApi;
using StaMemory.Services;

namespace StaMemory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RankingController : ControllerBase
{
    private readonly IRankingService _rankingService;

    public RankingController(IRankingService rankingService) : base()
    {
        _rankingService = rankingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRankingAsync(GetRanking.Request request)
    {
        var result = await _rankingService.GetRankingAsync(request.SeasonId);

        return Ok(result);
    }
}
