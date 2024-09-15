using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaMemory.Database;

public class Player
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string PlayerId { get; set; } = null!;

    [BsonElement("playerName")]
    public string PlayerName { get; set; } = null!;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}
