using Microsoft.AspNetCore.Mvc;
using StaMemory.ProtocolModels.Common;
using StaMemory.ProtocolModels.PlayerApi;
using StaMemory.Services;

namespace StaMemory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService) : base()
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayerAsync(RequestHeader requestHeader)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        return player is null
            ? NotFound()
            : Ok(new GetPlayer.Response { PlayerName = player.PlayerName });
    }

    [HttpPost]
    public async Task<IActionResult> RegisterPlayerAsync(RegisterPlayer.Request request)
    {
        var playerId = await _playerService.RegisterPlayerAsync(request.PlayerName);

        return Ok(new RegisterPlayer.Response { PlayerId = playerId });
    }

    [HttpPut]
    public async Task<IActionResult> RenamePlayerAsync(RequestHeader requestHeader, RenamePlayer.Request request)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        if (player is null)
        {
            return NotFound();
        }

        await _playerService.RenamePlayerAsync(player, request.PlayerName);

        return Ok();
    }
}
