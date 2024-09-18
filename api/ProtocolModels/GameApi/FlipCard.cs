using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StaMemory.ProtocolModels.GameApi;

public class FlipCard
{
    public class Request
    {
        [BindRequired]
        public int CardIndex { get; set; }

        [Required]
        [MaxLength(Constants.Length.Token)]
        public string Token { get; set; } = null!;
    }

    public class Response
    {
        public string Token { get; set; } = null!;

        public int FlippedCardIndex { get; set; }

        public int FlippedCardId { get; set; }

        public bool IsMatched { get; set; }

        public bool IsCleared { get; set; }

        public int? ClearTime { get; set; }
    }
}
