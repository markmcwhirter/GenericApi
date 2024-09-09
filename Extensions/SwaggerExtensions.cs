using FastEndpoints.Swagger;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenericApi.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services) =>
            services
               .SwaggerDocument(o =>
               {
                   o.EnableJWTBearerAuth = false;
                   //o.DocumentSettings = s =>
                   //{
                   //    s.AddAuth(ApikeyAuth.SchemeName, new()
                   //    {
                   //        Name = ApikeyAuth.HeaderName,
                   //        In = OpenApiSecurityApiKeyLocation.Header,
                   //        Type = OpenApiSecuritySchemeType.ApiKey,
                   //    });
                   //};
               });
    }
}
