namespace GenericApi;

using Serilog;

public static class LogConfig
{
    public static void ConfigureLogger(IConfigurationRoot? configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Application", "GenericApi")
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production")
            .Enrich.WithProperty("ThreadName", Thread.CurrentThread.Name ?? "Unnamed Thread")
            .Enrich.WithProcessId()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithClientIp()
            .Enrich.WithRequestHeader("User-Agent")
            .WriteTo.File("logs/log-.txt",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .WriteTo.Seq(configuration.GetValue<string>("Seq"))
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}
