using Component.Matching.Configuration;
using Component.Matching.Contracts;
using Component.Matching.Models;
using Component.Matching.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Component.Matching.DependencyInjection
{
    public static class MatchingServiceInjectionExtension
    {
        public static IServiceCollection AddMatchingService(this IServiceCollection container, IConfiguration configuration)
        {
            container.Configure<MatchingConfig>(configuration.GetSection("MatchingConfig"));
            container.Configure<TypoDetectorConfig>(configuration.GetSection("TypoDetectorConfig"));
            container.AddSingleton<INameSimilarity,InitialsMatchingService>();
            container.AddSingleton<INameSimilarity, TypoDetectorService>();
            container.AddSingleton<INameSimilarity, NickNameDetectorService>();
            container.AddTransient(typeof(IMatchingService<Person>), typeof(MatchingService<Person>));
            
            return container;
        }

        public static void UseMatchingRules(this IApplicationBuilder app, IOptions<MatchingConfig> options) => 
            MatchingRules.SetRange(options.Value.MatchingRules.ToArray());
    }
}
