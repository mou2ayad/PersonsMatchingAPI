using System;
using Component.Matching.Configuration;
using Component.Matching.DependencyInjection;
using Component.Utilities.DependencyInjection;
using Component.Utilities.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Component.Utilities.ErrorHandling;

namespace Api.Matching
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string API_NAME = "MatchingApi";

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddHealthChecks();
            services.AddMemoryCache()
                .AddMatchingService(Configuration)
                .AddSwaggerService(API_NAME, Configuration);

        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> log, IWebHostEnvironment env, IServiceProvider container)
        {
            app.UseSwaggerMiddleware(API_NAME, Configuration).
                UseErrorHandler(log, API_NAME, env.IsDevelopment())
                .UseHttpsRedirection()
                .UseRouting()
                .UseHealthChecks(new Microsoft.AspNetCore.Http.PathString("/healthcheck"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMatchingRules(container.GetService<IOptions<MatchingConfig>>());
        }
    }
}
