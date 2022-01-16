using System;
using System.Text.RegularExpressions;
using Component.Matching.Contracts;
using Component.Matching.Models;

namespace Component.Matching.Services
{
    public class InitialsMatchingService : INameSimilarity
    {
        public bool Match(string name, string anotherName)
            => PossibleInitial(name, anotherName);

        public SimilarityServiceType Type => SimilarityServiceType.Initials;

        private bool PossibleInitial(string name, string anotherName)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(anotherName))
                return false;
            string nameInitial = GetInitial(name);
            string anotherNameInitial= GetInitial(anotherName);
            if (string.IsNullOrEmpty(nameInitial) || string.IsNullOrEmpty(anotherNameInitial))
                return false;
            return nameInitial.Equals(anotherName, StringComparison.OrdinalIgnoreCase) ||
                   anotherNameInitial.Equals(name, StringComparison.OrdinalIgnoreCase);
        }
        protected bool IsInitial(string name)
        {
            Regex rg = new Regex(@"^[A-Za-z]\.$", RegexOptions.IgnoreCase);
            return rg.IsMatch(name);
        }

        protected string RemoveWhiteSpaces(string text)
            => Regex.Replace(text, @"\s", "");

        protected string GetInitial(string name)
        {
            string cleanName = RemoveWhiteSpaces(name);
            if (cleanName.Length == 0)
                return null;
            return IsInitial(cleanName) ? cleanName : cleanName[0] + ".";
        }
    }
}
