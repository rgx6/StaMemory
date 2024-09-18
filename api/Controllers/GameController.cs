using System.Net;
using Microsoft.AspNetCore.Mvc;
using StaMemory.ProtocolModels.Common;
using StaMemory.ProtocolModels.GameApi;
using StaMemory.Services;

namespace StaMemory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly IDifficultyService _difficultyService;
    private readonly IGameService _gameService;
    private readonly IPlayerService _playerService;

    public GameController(
        IDifficultyService difficultyService,
        IGameService gameService,
        IPlayerService playerService) : base()
    {
        _difficultyService = difficultyService;
        _gameService = gameService;
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayingGameAsync(RequestHeader requestHeader)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        if (player is null)
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        var playingGame = await _gameService.GetPlayingGameAsync(requestHeader.PlayerId);

        if (playingGame is null)
        {
            return NoContent();
        }

        var response = new GetPlayingGame.Response
        {
            Width = playingGame.Width,
            Turn = playingGame.Turn,
            Token = playingGame.Token,
            FirstCardIndex = playingGame.FirstCardIndex,
            FirstCardId = playingGame.FirstCardIndex is null
                ? null
                : playingGame.Cards[playingGame.FirstCardIndex.Value].CardId,
            Cards = playingGame.Cards.Select(x => new GetPlayingGame.Response.Card
            {
                CardId = x.IsOpen ? x.CardId : 0,
                IsOpen = x.IsOpen,
            }).ToList(),
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewGameAsync(RequestHeader requestHeader, CreateNewGame.Request request)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        if (player is null)
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        var playingGame = await _gameService.GetPlayingGameAsync(requestHeader.PlayerId);

        if (playingGame is not null)
        {
            return Conflict();
        }

        var difficulty = await _difficultyService.GetDifficultyAsync(request.DifficultyId);

        if (difficulty is null)
        {
            return BadRequest();
        }

        await _gameService.CreateNewGameAsync(requestHeader.PlayerId, difficulty);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> FlipCardAsync(RequestHeader requestHeader, FlipCard.Request request)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        if (player is null)
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        var playingGame = await _gameService.GetPlayingGameAsync(requestHeader.PlayerId);

        if (playingGame is null)
        {
            return NotFound();
        }

        if (playingGame.Token != request.Token)
        {
            return BadRequest();
        }

        (var isError, var result) = await _gameService.FlipCardAsync(playingGame, request.CardIndex);

        if (isError)
        {
            return BadRequest();
        }

        var response = new FlipCard.Response
        {
            Token = result!.Token,
            FlippedCardIndex = result.FlippedCardIndex,
            FlippedCardId = result.FlippedCardId,
            IsMatched = result.IsMatched,
            IsCleared = result.IsCleared,
            ClearTime = result.ClearTime,
        };

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> GiveUpGameAsync(RequestHeader requestHeader, GiveUpGame.Request request)
    {
        var player = await _playerService.GetPlayerAsync(requestHeader.PlayerId);

        if (player is null)
        {
            return StatusCode((int)HttpStatusCode.Forbidden);
        }

        var playingGame = await _gameService.GetPlayingGameAsync(requestHeader.PlayerId);

        if (playingGame is null)
        {
            return NotFound();
        }

        if (playingGame.Token != request.Token)
        {
            return BadRequest();
        }

        await _gameService.GiveUpGameAsync(playingGame);

        return Ok();
    }
}
