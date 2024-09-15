namespace StaMemory.ProtocolModels.DifficultyApi;

public class GetDifficultyList
{
    public class Response
    {
        public IList<Difficulty> DifficultyList { get; set; } = null!;

        public class Difficulty
        {
            public int DifficultyId { get; set; }

            public string DifficultyName { get; set; } = null!;

            public int Width { get; set; }

            public int Height { get; set; }
        }
    }
}
