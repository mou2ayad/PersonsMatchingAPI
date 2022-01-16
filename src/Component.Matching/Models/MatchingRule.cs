using System.Collections.Generic;

namespace Component.Matching.Models
{
    public class MatchingRule
    {
        public MatchingRule()
        {
        }

        public MatchingRule(string propertyName, decimal matchingScore)
        {
            PropertyName = propertyName;
            MatchingScore = matchingScore;
        }

        public static MatchingRule From(string propertyName, decimal matchingScore) 
            => new MatchingRule(propertyName, matchingScore);

        public MatchingRule With(params SimilarityRule[] similarityRules)
        {
            SimilarityRules = similarityRules;
            return this;
        }
        public string PropertyName { set; get; }
        public decimal MatchingScore { set; get; }

        public IEnumerable<SimilarityRule> SimilarityRules { set; get; }
    }
}
