namespace StaMemory.ProtocolModels.GameApi;

public class GetPlayingGame
{
    public class Response
    {
        public int Width { get; set; }

        public int Turn { get; set; }

        public string Token { get; set; } = null!;

        public int? FirstCardIndex { get; set; }

        public int? FirstCardId { get; set; }

        public IList<Card> Cards { get; set; } = null!;

        public class Card
        {
            public int CardId { get; set; }

            public bool IsOpen { get; set; }
        }
    }
}
