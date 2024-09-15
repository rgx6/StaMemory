using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaMemory.Database;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string GameId { get; set; } = null!;

    [BsonElement("seasonId")]
    public int SeasonId { get; set; }

    [BsonElement("playerId")]
    public string PlayerId { get; set; } = null!;

    [BsonElement("difficultyId")]
    public int DifficultyId { get; set; }

    [BsonElement("difficultyName")]
    public string DifficultyName { get; set; } = null!;

    [BsonElement("width")]
    public int Width { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = null!;

    [BsonElement("turn")]
    public int Turn { get; set; }

    [BsonElement("token")]
    public string Token { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("completedAt")]
    public DateTime? CompletedAt { get; set; }

    [BsonElement("firstCardIndex")]
    public int? FirstCardIndex { get; set; }

    [BsonElement("cards")]
    public IList<Card> Cards { get; set; } = null!;
}
