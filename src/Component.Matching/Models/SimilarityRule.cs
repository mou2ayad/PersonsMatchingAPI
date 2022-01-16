namespace Component.Matching.Models
{
    public class SimilarityRule
    {
        public SimilarityRule()
        {
            
        }
        public SimilarityRule(SimilarityServiceType similarityType, decimal similarityScore)
        {
            SimilarityType = similarityType;
            SimilarityScore = similarityScore;
        }

        public static SimilarityRule From(SimilarityServiceType similarityType, decimal similarityScore) => 
            new SimilarityRule(similarityType, similarityScore);

        public SimilarityServiceType SimilarityType { set; get; }
        public decimal SimilarityScore { set; get; }
    }
}