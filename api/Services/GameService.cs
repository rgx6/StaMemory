using System.Security.Cryptography;
using StaMemory.Database;
using StaMemory.Models;
using StaMemory.Repositories;

namespace StaMemory.Services;

public interface IGameService
{
    Task<Game?> GetPlayingGameAsync(string playerId);
    Task CreateNewGameAsync(string playerId, Difficulty difficulty);
    Task<(bool isError, FlipCardResult?)> FlipCardAsync(Game game, int cardIndex);
    Task GiveUpGameAsync(Game game);
}

public class GameService : IGameService
{
    private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
    private static readonly IList<int> _cardIdMaster = Enumerable.Range(1, 125).ToList();

    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Game?> GetPlayingGameAsync(string playerId)
    {
        var currentSeason = await _unitOfWork.SeasonRepository.GetCurrentSeasonAsync();

        if (currentSeason is null)
        {
            throw new Exception("currentSeason is null");
        }

        return await _unitOfWork.GameRepository.GetPlayingGameAsync(playerId, currentSeason.SeasonId);
    }

    public async Task CreateNewGameAsync(string playerId, Difficulty difficulty)
    {
        var currentSeason = await _unitOfWork.SeasonRepository.GetCurrentSeasonAsync();

        if (currentSeason is null)
        {
            throw new Exception("currentSeason is null");
        }

        var game = new Game
        {
            GameId = Utils.IssueId(),
            SeasonId = currentSeason.SeasonId,
            PlayerId = playerId,
            DifficultyId = difficulty.DifficultyId,
            DifficultyName = difficulty.DifficultyName,
            Width = difficulty.Width,
            Status = Constants.GameStatus.Playing,
            Turn = 1,
            Token = Utils.IssueId(),
            CreatedAt = DateTime.UtcNow,
            CompletedAt = null,
            FirstCardIndex = null,
            Cards = InitCards(difficulty),
        };

        await _unitOfWork.GameRepository.RegisterGameAsync(game);

        await _unitOfWork.CommitAsync();
    }

    public async Task<(bool isError, FlipCardResult?)> FlipCardAsync(Game game, int cardIndex)
    {
        if (cardIndex < 0 || game.Cards.Count <= cardIndex)
        {
            return (true, null);
        }

        var card = game.Cards[cardIndex];

        if (card.IsOpen)
        {
            return (true, null);
        }

        if (game.FirstCardIndex is null)
        {
            // 1枚目の処理

            game.Token = Utils.IssueId();
            game.FirstCardIndex = cardIndex;

            await _unitOfWork.CommitAsync();

            return (false, new FlipCardResult
            {
                Token = game.Token,
                FlippedCardIndex = cardIndex,
                FlippedCardId = card.CardId,
                IsMatched = false,
                IsCleared = false,
                ClearTime = null,
            });
        }

        // 2枚目の処理

        if (game.FirstCardIndex.Value == cardIndex)
        {
            return (true, null);
        }

        var firstCard = game.Cards[game.FirstCardIndex.Value];
        var secondCard = game.Cards[cardIndex];

        if (firstCard.CardId != secondCard.CardId)
        {
            game.Token = Utils.IssueId();
            game.Turn += 1;
            game.FirstCardIndex = null;

            await _unitOfWork.CommitAsync();

            return (false, new FlipCardResult
            {
                Token = game.Token,
                FlippedCardIndex = cardIndex,
                FlippedCardId = card.CardId,
                IsMatched = false,
                IsCleared = false,
                ClearTime = null,
            });
        }

        game.FirstCardIndex = null;
        firstCard.IsOpen = true;
        secondCard.IsOpen = true;

        if (game.Cards.Any(x => !x.IsOpen))
        {
            // 継続
            game.Token = Utils.IssueId();
            game.Turn += 1;

            await _unitOfWork.CommitAsync();

            return (false, new FlipCardResult
            {
                Token = game.Token,
                FlippedCardIndex = cardIndex,
                FlippedCardId = card.CardId,
                IsMatched = true,
                IsCleared = false,
                ClearTime = null,
            });
        }

        // 終了処理
        game.Status = Constants.GameStatus.Clear;
        game.CompletedAt = DateTime.UtcNow;

        await _unitOfWork.CommitAsync();

        return (false, new FlipCardResult
        {
            Token = game.Token,
            FlippedCardIndex = cardIndex,
            FlippedCardId = card.CardId,
            IsMatched = true,
            IsCleared = true,
            ClearTime = Utils.GetClearTime(game.CreatedAt, game.CompletedAt.Value),
        });
    }

    public async Task GiveUpGameAsync(Game game)
    {
        game.Status = Constants.GameStatus.GiveUp;
        game.Token = Utils.IssueId();
        game.CompletedAt = DateTime.UtcNow;

        await _unitOfWork.CommitAsync();
    }

    private static List<Card> InitCards(Difficulty difficulty)
    {
        var cardIdMaster = new List<int>(_cardIdMaster);

        ShuffleCard(cardIdMaster);

        var cardIdList = new List<int>();

        var numberOfCards = difficulty.Width * difficulty.Height;

        for (var i = 0; i < numberOfCards / 2; i++)
        {
            var temp = cardIdMaster[i];

            cardIdList.Add(temp);
            cardIdList.Add(temp);
        }

        ShuffleCard(cardIdList);

        return cardIdList
            .Select(cardId => new Card { CardId = cardId, IsOpen = false })
            .ToList();
    }

    private static void ShuffleCard(IList<int> cardIdList)
    {
        for (var i = 0; i < cardIdList.Count - 1; i++)
        {
            var j = GetRandomInt(i, cardIdList.Count - 1);

            var temp = cardIdList[i];
            cardIdList[i] = cardIdList[j];
            cardIdList[j] = temp;
        }
    }

    private static int GetRandomInt(int min, int max)
    {
        var range = max - min + 1;

        var bytes = new byte[4];

        _rng.GetBytes(bytes);

        var generatedValue = BitConverter.ToInt32(bytes, 0) & int.MaxValue;

        return min + (generatedValue % range);
    }
}
