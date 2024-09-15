using System.ComponentModel.DataAnnotations;

namespace StaMemory.ProtocolModels.GameApi;

public class GiveUpGame
{
    public class Request
    {
        [Required]
        [MaxLength(Constants.Length.Token)]
        public string Token { get; set; } = null!;
    }
}
