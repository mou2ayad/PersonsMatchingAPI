using FluentAssertions;
using Component.Matching.Configuration;
using Component.Matching.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Test.NamesSimilarity
{
    public class TypoDetectorServiceShould
    {
        [TestCase(1, "Andrew", "Andreew", Description = "With insertions (1 letter) and maxDistance is 1")]
        [TestCase(2, "Andrew", "Anndreew", Description = "With insertions (2 letters) and maxDistance is 2")]
        [TestCase(1, "Andrew", "Andrw", Description = "With deletions (1 letter) and maxDistance is 1")]
        [TestCase(2, "Andrew", "Andr", Description = "With deletions (2 letters) and maxDistance is 2")]
        [TestCase(2, "Andrew", "Andrw", Description = "With deletions (1 letter) is less than maxDistance (1)")]
        [TestCase(1, "Andrew", "Androw", Description = "With substitutions (1 letter) and maxDistance is 1")]
        [TestCase(2, "Andrew", "Andrwe", Description = "With substitutions (2 letters) and maxDistance is 2")]
        public void Detect_typo_with_deletions(int maxDistance, string correctName, string writtenName)
        {
            var sut = Sut(maxDistance);

            var possibleTypo = sut.Match(correctName, writtenName);

            possibleTypo.Should().Be(true);
        }

        [TestCase(1, "Andrew", "Anndreew", Description = "With insertions when distance(2) is more than maxDistance(1)")]
        [TestCase(1, "Andrew", "Andr", Description = "With deletions (2 letters) and maxDistance is 1")]
        [TestCase(1, "Andrew", "Androe", Description = "With substitutions when distance (2) is more than maxDistance (1)")]
        [TestCase(1, "Andrew", "andrew", Description = "When the diff is only letter case")]

        public void Detect_NO_typo_with_insertions(int maxDistance, string correctName, string writtenName)
        {
            var sut = Sut(maxDistance);

            var possibleTypo = sut.Match(correctName, writtenName);

            possibleTypo.Should().Be(false);
        }

        [Test]
        public void Max_distance_be_one_if_no_config()
        {
            var sut = SutTestable();

            var defaultMaxDistance = sut.GetMaxDistance();

            defaultMaxDistance.Should().Be(1);
        }


        private static TypoDetectorService Sut(int minDistance) =>
            new (Options.Create(new TypoDetectorConfig { MaxDistance = minDistance }));

        private static SpyTypoDetectorService SutTestable() => new();
    }

    public class SpyTypoDetectorService : TypoDetectorService
    {
        public SpyTypoDetectorService() :base(Options.Create(new TypoDetectorConfig()))
        {
        }
        public int GetMaxDistance() => _config.MaxDistance;
    }

}
