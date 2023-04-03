using Serilog;
using TZN.SU.CSGOTournamentDiscordBot;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("AppData/Logs/.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

await Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseConsoleLifetime()
    .ConfigureServices((context, services) =>
    {
        var configurationRoot = context.Configuration;

        services.Configure<BotOptions>(configurationRoot.GetSection(nameof(BotOptions)));
        
        services.AddLogging(logging => logging.ClearProviders().AddSerilog());
        services.AddHostedService<BotService>();
        services.AddTransient<TeamsService>();
    })
    .RunConsoleAsync();

await Log.CloseAndFlushAsync();