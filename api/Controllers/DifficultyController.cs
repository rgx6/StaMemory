using Microsoft.AspNetCore.Mvc;
using StaMemory.ProtocolModels.DifficultyApi;
using StaMemory.Services;

namespace StaMemory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DifficultyController : ControllerBase
{
    private readonly IDifficultyService _difficultyService;

    public DifficultyController(IDifficultyService difficultyService) : base()
    {
        _difficultyService = difficultyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDifficultyListAsync()
    {
        var difficultyList = await _difficultyService.GetDifficultyListAsync();

        var response = new GetDifficultyList.Response
        {
            DifficultyList = difficultyList.Select(x => new GetDifficultyList.Response.Difficulty
            {
                DifficultyId = x.DifficultyId,
                DifficultyName = x.DifficultyName,
                Width = x.Width,
                Height = x.Height,
            }).ToList(),
        };

        return Ok(response);
    }
}
