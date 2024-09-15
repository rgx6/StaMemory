using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StaMemory.Database;

public class Season
{
    [BsonId]
    [BsonRepresentation(BsonType.Int32)]
    public int SeasonId { get; set; }

    [BsonElement("seasonName")]
    public string SeasonName { get; set; } = null!;

    [BsonElement("isCurrentSeason")]
    public bool IsCurrentSeason { get; set; }
}
