using System.Collections.Generic;

namespace Component.Matching.Models
{
    public static class MatchingRules
    {
        private static readonly Dictionary<string,MatchingRule> Rules = new Dictionary<string,MatchingRule>();

        public static void SetRange(params MatchingRule[] matchingRule)
        {
            if(matchingRule==null) return;
            foreach (var rule in matchingRule)
                Set(rule);
        }
        public static void Set(MatchingRule rule)
        {
            if (!Rules.TryAdd(rule.PropertyName, rule))
                Rules[rule.PropertyName] = rule;
        }

        public static MatchingRule Get(string propertyName)
        {
            if (Rules.TryGetValue(propertyName, out MatchingRule value))
                return value;
            return null;
        }

    }
}