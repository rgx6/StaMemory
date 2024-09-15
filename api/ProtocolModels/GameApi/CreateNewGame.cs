using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StaMemory.ProtocolModels.GameApi;

public class CreateNewGame
{
    public class Request
    {
        [BindRequired]
        public int DifficultyId { get; set; }
    }
}
