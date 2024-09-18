namespace StaMemory.Models;

public class FlipCardResult
{
    public string Token { get; set; } = null!;

    public int FlippedCardIndex { get; set; }

    public int FlippedCardId { get; set; }

    public bool IsMatched { get; set; }

    public bool IsCleared { get; set; }

    public int? ClearTime { get; set; }
}
