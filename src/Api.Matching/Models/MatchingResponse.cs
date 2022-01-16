namespace Api.Matching.Models
{
    public class MatchingResponse
    {
        public static MatchingResponse From(decimal matchingScore)
            => new() {MatchingScore = matchingScore};
        public decimal MatchingScore { set; get; }
    }
}
