using System.ComponentModel.DataAnnotations;

namespace StaMemory.ProtocolModels.PlayerApi;

public class RegisterPlayer
{
    public class Request
    {
        [Required]
        [MaxLength(Constants.Length.PlayerName)]
        public string PlayerName { get; set; } = null!;
    }

    public class Response
    {
        public string PlayerId { get; set; } = null!;
    }
}
