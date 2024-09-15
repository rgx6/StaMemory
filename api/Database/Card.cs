using MongoDB.Bson.Serialization.Attributes;

namespace StaMemory.Database;

public class Card
{
    [BsonElement("cardId")]
    public int CardId { get; set; }

    [BsonElement("isOpen")]
    public bool IsOpen { get; set; }
}
