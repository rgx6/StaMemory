using Microsoft.AspNetCore.Mvc;
using StaMemory.ProtocolModels.SeasonApi;
using StaMemory.Services;

namespace StaMemory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeasonController : ControllerBase
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService) : base()
    {
        _seasonService = seasonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSeasonListAsync()
    {
        var seasonList = await _seasonService.GetSeasonListAsync();

        var response = new GetSeasonList.Response
        {
            SeasonList = seasonList.Select(x => new GetSeasonList.Response.Season
            {
                SeasonId = x.SeasonId,
                SeasonName = x.SeasonName,
            }).ToList(),
        };

        return Ok(response);
    }
}
