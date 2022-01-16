using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Component.Matching.Configuration;
using Component.Matching.Contracts;
using Component.Matching.Models;
using Component.Matching.Services;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Test.Matching
{
    public class MatchingServiceShould
    {
        public MatchingServiceShould() => FillMatchingRules();

        [Test]
        public async Task Match_first_and_last_names_onlyAsync()
        {
            var sut = Sut();
            var firstPerson = PersonBuilder.Create("Andrew", "Craw").With(DateTime.Parse("1985-02-20")).Build();
            var secondPerson = PersonBuilder.Create("Andrew", "Craw").Build();

            decimal matchingScore =await sut.Match(firstPerson, secondPerson);

            matchingScore.Should().Be(60);
        }
        [Test]
        public async Task Match_Date_of_Birth_onlyAsync()
        {
            var sut = Sut();
            var firstPerson = PersonBuilder.Create("Andrew", "Craw").With(DateTime.Parse("1985-02-20")).Build();
            var secondPerson = PersonBuilder.Create("Petty", "Smith").With(DateTime.Parse("1985-02-20")).Build();

            decimal matchingScore = await sut.Match(firstPerson, secondPerson);

            matchingScore.Should().Be(40);
        }

        [TestCase("A.",Description = "similarity in first name (initials)")]
        [TestCase("Andew", Description = "similarity in first name (typo)")]
        [TestCase("Andy", Description = "similarity in first name (diminutive)")]
        public async Task Match_Date_of_Birth_and_last_name_with_similar_firstNameAsync(string secondPersonName)
        {
            var sut = Sut();
            var firstPerson = PersonBuilder.Create("Andrew", "Craw").With(DateTime.Parse("1985-02-20")).Build();
            var secondPerson = PersonBuilder.Create(secondPersonName, "Craw").With(DateTime.Parse("1985-02-20")).Build();

            decimal matchingScore = await sut.Match(firstPerson, secondPerson);

            matchingScore.Should().Be(95);
        }

        [Test]
        public async Task Match_identification_number_mainlyAsync()
        {
            var sut = Sut();
            var firstPerson = PersonBuilder.Create("Andrew", "Craw").With(DateTime.Parse("1985-02-20")).With("931212312").Build();
            var secondPerson = PersonBuilder.Create("Petty", "Smith").With(DateTime.Parse("1985-02-20")).With("931212312").Build();

            decimal matchingScore =await sut.Match(firstPerson, secondPerson);

            matchingScore.Should().Be(100);
        }

        [TestCase("A.",8, Description = "similarity in first name (initials) with 8 as expected score")]
        [TestCase("Andew",15, Description = "similarity in first name (typo) with 15 as expected score")]
        [TestCase("Andy",10, Description = "similarity in first name (diminutive) with 10 as expected score")]
        public async Task Match_similar_firstName_with_diff_similarity_rules_valuesAsync(string secondPersonName,decimal expectedMatchScore)
        {
            OverrideSimilarityRulesValues();
            var sut = Sut();
            var firstPerson = PersonBuilder.Create("Andrew", "Craw").Build();
            var secondPerson = PersonBuilder.Create(secondPersonName, "Smith").Build();

            decimal matchingScore = await sut.Match(firstPerson, secondPerson);

            matchingScore.Should().Be(expectedMatchScore);
        }

        public static MatchingService<Person> Sut()
            => new(GetSimilarityServices());

        private static IEnumerable<INameSimilarity> GetSimilarityServices() =>
            new List<INameSimilarity>
            {
                new TypoDetectorService(Options.Create(new TypoDetectorConfig { MaxDistance = 1 })),
                new NickNameDetectorService(),
                new InitialsMatchingService()
            };
        
        private static void FillMatchingRules()
        {
            MatchingRules.SetRange(
                MatchingRule.From("FirstName", 20).With(
                    SimilarityRule.From(SimilarityServiceType.Initials, 15),
                    SimilarityRule.From(SimilarityServiceType.NickName, 15),
                    SimilarityRule.From(SimilarityServiceType.Typo, 15)
                ),
                MatchingRule.From("LastName", 40),
                MatchingRule.From("DateOfBirth", 40),
                MatchingRule.From("IdentificationNumber", 100));
        }

        private static void OverrideSimilarityRulesValues()
        {
            MatchingRules.Set(
                MatchingRule.From("FirstName", 20).With(
                    SimilarityRule.From(SimilarityServiceType.Initials, 8),
                    SimilarityRule.From(SimilarityServiceType.NickName, 10),
                    SimilarityRule.From(SimilarityServiceType.Typo, 15)
                ));
        }
    }
}
