using System;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Api.Matching;
using Api.Matching.Models;
using API.Test.Utils;
using Component.Matching.Models;
using Component.Utilities.ErrorHandling;
using Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace API.Test
{
    public class MatchingShould
    {
       
        [Test]
        public async Task Match_two_persons_successfully()
        {
            var testClient = HttpClient();

            var matchingResponse = await testClient.Post<MatchingResponse>("api/v1/persons/Match",
                MatchRequestExample());

            matchingResponse.Should().NotBeNull();
            matchingResponse.MatchingScore.Should().Be(55);
        }

        [Test]
        public async Task Match_two_persons_successfully_and_ignore_similarity_rules_if_not_added_to_config()
        {
            var testClient = HttpClient();

            ClearMatchingRules();
            var matchingResponse = await testClient.Post<MatchingResponse>("api/v1/persons/Match",
                MatchRequestExample());

            matchingResponse.Should().NotBeNull();
            matchingResponse.MatchingScore.Should().Be(40);
        }

        [Test]
        public async Task Match_two_persons_successfully_and_with_custom_similarity_rules_value()
        {
            var testClient = HttpClient();

            ChangeNickNameSimilarityRuleScore(5);
            var matchingResponse = await testClient.Post<MatchingResponse>("api/v1/persons/Match",
                MatchRequestExample());

            matchingResponse.Should().NotBeNull();
            matchingResponse.MatchingScore.Should().Be(45);
        }

        [Test]
        public async Task Match_give_bad_request_when_one_of_two_persons_is_null()
        {
            var testClient = HttpClient();

            var request = MatchRequestExample();
            request.First = null;
            var responseMessage = await testClient.Post("api/v1/persons/Match", request);

            var errorDetails = await responseMessage.Content.ReadFromJsonAsync<ExceptionDetails>();

            responseMessage.StatusCode.Should().Be(400);
            errorDetails.Should().NotBeNull();
            errorDetails.ErrorMessage.Should().Be("Both First and Second Person can't be null");
        }

        [Test]
        public async Task Match_give_bad_request_when_both_persons_are_null()
        {
            var testClient = HttpClient();

            var request = MatchRequestExample();
            request.First = null;
            request.Second = null;
            var responseMessage = await testClient.Post("api/v1/persons/Match", request);

            var errorDetails = await responseMessage.Content.ReadFromJsonAsync<ExceptionDetails>();

            responseMessage.StatusCode.Should().Be(400);
            errorDetails.Should().NotBeNull();
            errorDetails.ErrorMessage.Should().Be("Both First and Second Person can't be null");
        }

        [Test]
        public async Task Match_give_bad_request_when_one_of_persons_has_no_first_name()
        {
            var testClient = HttpClient();

            var request = MatchRequestExample();
            request.First=PersonBuilder.Create(null,"someLastName").Build();
            var responseMessage = await testClient.Post("api/v1/persons/Match", request);

            var errorDetails = await responseMessage.Content.ReadFromJsonAsync<ExceptionDetails>();

            responseMessage.StatusCode.Should().Be(400);
            errorDetails.Should().NotBeNull();
            errorDetails.ErrorMessage.Should().Be("FirstName and Last Name can't not be null of empty");
        }

        [Test]
        public async Task Match_give_bad_request_when_both_of_persons_have_no_last_names()
        {
            var testClient = HttpClient();
            var first = PersonBuilder.Create("andy", null).Build();
            var second = PersonBuilder.Create("andy", null).Build();

            var responseMessage = await testClient.Post("api/v1/persons/Match", MatchingRequest.From(first,second));

            var errorDetails = await responseMessage.Content.ReadFromJsonAsync<ExceptionDetails>();

            responseMessage.StatusCode.Should().Be(400);
            errorDetails.Should().NotBeNull();
            errorDetails.ErrorMessage.Should().Be("FirstName and Last Name can't not be null of empty");
        }

        
        private static IWebHostBuilder CreateWebHost() =>
            new WebHostBuilder()
                .UseStartup<Startup>()
                .UseWebRoot(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "wwwroot")))
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(TestContext.CurrentContext.TestDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json")
                    .Build()
                );

        public static TestClient Create(Action<IServiceCollection> overrideInjections)
        {
            var server = new TestServer(
                CreateWebHost()
                    .ConfigureTestServices(overrideInjections));
            return new TestClient(server);
        }

        private static TestClient HttpClient() => Create(container =>
        {
            // in case we want to override some services in the container , we can do here
        });

        private static void ClearMatchingRules() => MatchingRules.Get("FirstName").SimilarityRules=null;

        private static void ChangeNickNameSimilarityRuleScore(decimal score) =>
            MatchingRules.Get("FirstName").SimilarityRules
                .First(e => e.SimilarityType == SimilarityServiceType.NickName).SimilarityScore = score;

        public static MatchingRequest MatchRequestExample() => new()
        {
            First = PersonBuilder.Create("Andrew", "Craw").With(DateTime.Parse("1985-02-20")).With("931212312").Build(),
            Second = PersonBuilder.Create("Andy", "Smith").With(DateTime.Parse("1985-02-20")).With("931212311").Build()
        };
    }
}