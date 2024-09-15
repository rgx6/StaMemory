using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace StaMemory.ProtocolModels.Common;

public class RequestHeader
{
    [FromHeader]
    [Required]
    [MaxLength(Constants.Length.PlayerId)]
    public string PlayerId { get; set; } = null!;
}
