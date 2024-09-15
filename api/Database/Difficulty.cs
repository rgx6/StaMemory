using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaMemory.Database;

public class Difficulty
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int DifficultyId { get; set; }

    [BsonElement("difficultyName")]
    public string DifficultyName { get; set; } = null!;

    [BsonElement("width")]
    public int Width { get; set; }

    [BsonElement("height")]
    public int Height { get; set; }
}
