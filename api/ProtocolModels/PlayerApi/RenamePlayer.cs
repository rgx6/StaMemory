using System.ComponentModel.DataAnnotations;

namespace StaMemory.ProtocolModels.PlayerApi;

public class RenamePlayer
{
    public class Request
    {
        [Required]
        [MaxLength(Constants.Length.PlayerName)]
        public string PlayerName { get; set; } = null!;
    }
}
