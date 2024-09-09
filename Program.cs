namespace GenericApi;

using FastEndpoints;
using FastEndpoints.Swagger;

using GenericApi.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using NSwag;

using Serilog;
using GenericApi.Repository;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

        LogConfig.ConfigureLogger(configuration);

        var builder = WebApplication.CreateBuilder(args);

        builder.WebHost.ConfigureKestrel((context, options) =>
        {
            options.ListenAnyIP(51806, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
                listenOptions.UseHttps();
            });
        });

        //builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        //builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        //builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
        builder.Services.AddProblemDetails();


        builder.Services
           .AddFastEndpoints()
           .AddAuthorization();

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddCors(options => { options.AddPolicy("corspolicy",
            policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCustomSwagger();

        builder.Services.AddCustomHealthChecks(configuration);

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContextFactory<NorthwindContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();   // This line is for development only
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        

        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

        builder.Host.UseSerilog(Log.Logger);

        // Build up application
        var app = builder.Build();

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()); // allow credentials

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseExceptionHandler();

        app.UseAuthorization();
        app.UseAuthentication();

        app.UseFastEndpoints(c =>
        {
            c.Versioning.Prefix = "v";
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}
