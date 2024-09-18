using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StaMemory.ProtocolModels.RankingApi;

public class GetRanking
{
    public class Request
    {
        [FromQuery]
        [BindRequired]
        public int SeasonId { get; set; }
    }

    public class Response
    {
        public IList<Ranking> RankingList { get; set; } = null!;

        public class Ranking
        {
            public string DifficultyName { get; set; } = null!;

            public IList<Score> ScoreList { get; set; } = null!;
        }

        public class Score
        {
            public string PlayerName { get; set; } = null!;

            public int Turn { get; set; }

            public int ClearTime { get; set; }

            public string ClearedAt { get; set; } = null!;
        }
    }
}
