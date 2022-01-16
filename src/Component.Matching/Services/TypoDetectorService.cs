using System;
using Component.Matching.Configuration;
using Component.Matching.Contracts;
using Component.Matching.Models;
using Microsoft.Extensions.Options;

namespace Component.Matching.Services
{
    public class TypoDetectorService :INameSimilarity
    {
        protected readonly TypoDetectorConfig _config;

        public TypoDetectorService(IOptions<TypoDetectorConfig> config) => _config = config.Value;

        public SimilarityServiceType Type => SimilarityServiceType.Typo;

        public bool Match(string name, string anotherName) => PossibleTypo(name, anotherName);

        private bool PossibleTypo(string name, string anotherName)
        {
            int distance= GetEditDistance(name, anotherName);
            return distance > 0 && distance <= _config.MaxDistance;
        }

        protected int GetEditDistance(string name, string anotherName) =>
            GetDamerauLevenshteinDistance(name.Trim().ToLower(), anotherName.Trim().ToLower());

        private int GetDamerauLevenshteinDistance(string name, string anotherName)
        {
            var bounds = new { Height = name.Length + 1, Width = anotherName.Length + 1 };

            var matrix = new int[bounds.Height, bounds.Width];

            for (var height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (var width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (var height = 1; height < bounds.Height; height++)
            {
                for (var width = 1; width < bounds.Width; width++)
                {
                    var cost = (name[height - 1] == anotherName[width - 1]) ? 0 : 1;
                    var insertion = matrix[height, width - 1] + 1;
                    var deletion = matrix[height - 1, width] + 1;
                    var substitution = matrix[height - 1, width - 1] + cost;

                    var distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && name[height - 1] == anotherName[width - 2] && name[height - 2] == anotherName[width - 1])
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);

                    matrix[height, width] = distance;
                }
            }
            return matrix[bounds.Height - 1, bounds.Width - 1];
        }
    }
}
