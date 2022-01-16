using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Component.Utilities.DependencyInjection
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services, string apiTitle, IConfiguration configuration)
        {

            OpenApiInfo swaggerDoc = new OpenApiInfo()
            {
                Title = apiTitle,
                Version = configuration.GetValue<string>("Swagger:Version"),
                Description = configuration.GetValue<string>("Swagger:Description")
            };
            if (configuration.GetSection("Swagger:Contact").Exists())
            {
                Uri.TryCreate(configuration.GetValue<string>("Swagger:Contact:Url"), UriKind.Absolute, out var uri);
                swaggerDoc.Contact = new OpenApiContact()
                {
                    Name = configuration.GetValue<string>("Swagger:Contact:Name"),
                    Email = configuration.GetValue<string>("Swagger:Contact:Email"),
                    Url = uri
                };
            }
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerDoc.Version, swaggerDoc);
                c.EnableAnnotations();
            });
            return services;
        }
    }
}
