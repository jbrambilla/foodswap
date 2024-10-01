using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

public static class SetupLogExtension
{
    public static void SetupSerilogMSSQl(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .Enrich.WithProperty("ApplicationName", "FoodSwap")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                },
                restrictedToMinimumLevel: LogEventLevel.Warning
            )
        );
    }
}