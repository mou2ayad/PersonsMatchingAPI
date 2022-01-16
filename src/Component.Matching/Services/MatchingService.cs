using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component.Matching.Contracts;
using Component.Matching.Models;

namespace Component.Matching.Services
{
    public class MatchingService<T> : IMatchingService<T> where T :IMatchable<T>
    {
        private readonly IEnumerable<INameSimilarity> _similarityServices;
        public MatchingService(IEnumerable<INameSimilarity> similarityServices)
            => _similarityServices = similarityServices;

        private INameSimilarity GetSimilarityService(SimilarityServiceType similarityServiceType)
            => _similarityServices.FirstOrDefault(e => e.Type == similarityServiceType);
        public Task<decimal> Match(T first, T second)
        {
            decimal matchingScore = 0;
            var propNames = first.OrderedPropertiesToMatch;
            Type t = typeof(T);
            foreach (var propName in propNames)
            {
                var firstValue=t.GetProperty(propName)?.GetValue(first, null);
                var secondValue = t.GetProperty(propName)?.GetValue(second, null);
                if(firstValue==null || secondValue==null)
                    continue;
                
                var rule = MatchingRules.Get(propName);
                if (firstValue.Equals(secondValue))
                    matchingScore += rule.MatchingScore;
                else if(rule.SimilarityRules!=null)
                {
                    foreach (var similarityRule in rule.SimilarityRules.OrderByDescending(e=>e.SimilarityScore))
                    {
                        var similarityService = GetSimilarityService(similarityRule.SimilarityType);
                        if (similarityService.Match(firstValue.ToString(), secondValue.ToString()))
                        {
                            matchingScore += similarityRule.SimilarityScore;
                            break;
                        }
                    }
                }
                if(matchingScore>=100)
                    break;
            }

            return Task.FromResult(matchingScore);
        }
    }
}
