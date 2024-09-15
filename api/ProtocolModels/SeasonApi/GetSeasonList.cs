namespace StaMemory.ProtocolModels.SeasonApi;

public class GetSeasonList
{
    public class Response
    {
        public IList<Season> SeasonList { get; set; } = null!;

        public class Season
        {
            public int SeasonId { get; set; }

            public string SeasonName { get; set; } = null!;
        }
    }
}
