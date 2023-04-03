using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Options;
using Serilog;

namespace TZN.SU.CSGOTournamentDiscordBot;

/// <summary>
/// Main service of the application.
/// </summary>
public class BotService : IHostedService
{
    private readonly ILogger<BotService> _logger;
    private readonly TeamsService _teamsService;
    private readonly IOptions<BotOptions> _options;
    
    private readonly DiscordClient _discordClient;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger service, injected by DI.</param>
    /// <param name="teamsService">Teams service, injected by DI.</param>
    /// <param name="options">Options object, injected by DI.</param>
    public BotService(ILogger<BotService> logger, TeamsService teamsService, IOptions<BotOptions> options)
    {
        _logger = logger;
        _teamsService = teamsService;
        _options = options;
        
        _discordClient = new(new()
        {
            Token = _options.Value.BotToken,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            LoggerFactory = new LoggerFactory().AddSerilog(),
            MinimumLogLevel = LogLevel.Debug,
            LogUnknownEvents = false
        });
    }

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting service.");
        
        await _discordClient.ConnectAsync();
        
        _logger.LogInformation("Connected.");

        _discordClient.GuildMemberAdded += UserJoined;
        
        _logger.LogInformation("Registered events.");
    }
    
    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping service.");
        
        await _discordClient.DisconnectAsync();
    }

    /// <summary>
    /// Event handler for when the user joins a guild.
    /// </summary>
    /// <param name="sender">Discord client object.</param>
    /// <param name="args">Event arguments.</param>
    private async Task UserJoined(DiscordClient sender, GuildMemberAddEventArgs args)
    {
        var userName = args.Member.Username + "#" + args.Member.Discriminator;
        
        _logger.LogInformation("User {userName} joined.", userName);

        if (args.Member.IsBot)
        {
            _logger.LogInformation("User {userName} is a bot.", userName);
            
            await Task.CompletedTask;
            return;
        }

        var team = _teamsService.GetTeamNameForMember(userName);
        
        if (team is null)
        {
            _logger.LogInformation("User {userName} is not in any team.", userName);
            
            await Task.CompletedTask;
            return;
        }
        
        _logger.LogInformation("User {userName} is in the team {team}.", userName, team);
        
        // Players role
        await CreateRoleIfNotExistsAndAssignUser(args.Guild, args.Member, _options.Value.PlayersRoleName, false, true, DiscordColor.Rose);
        
        // Team role
        await CreateRoleIfNotExistsAndAssignUser(args.Guild, args.Member, team, true, true);
        
        if (!_teamsService.UserIsCaptain(userName))
        {
            _logger.LogInformation("User {userName} is not a captain of a team.", userName);
            
            await Task.CompletedTask;
            return;
        }
        
        _logger.LogInformation("User {userName} is a captain of a team.", userName);
        
        // Captain role
        await CreateRoleIfNotExistsAndAssignUser(args.Guild, args.Member, _options.Value.CaptainsRoleName, false, true, DiscordColor.Turquoise);
    }

    /// <summary>
    /// Creates role, if it doesn't exist within the guild and assigns user to the role.
    /// </summary>
    /// <param name="guild">Guild object.</param>
    /// <param name="user">User the role is assigned to.</param>
    /// <param name="roleName">Name of the role to create, trimmed to 100 characters.</param>
    /// <param name="hoisted">Whether members of the role should be displayed separately on the sidebar.</param>
    /// <param name="mentionable">Whether the role is mentionable.</param>
    /// <param name="position">Position of the role.</param>
    /// <param name="color">Color of the role, will be randomly picked if omitted.</param>
    private async Task CreateRoleIfNotExistsAndAssignUser(DiscordGuild guild, DiscordMember user, string roleName, 
        bool hoisted, bool mentionable, DiscordColor? color = null)
    {
        var trimmedRoleName = roleName.Length > 100 ? roleName[..100] : roleName;
        
        var role = guild.Roles.FirstOrDefault(role => role.Value.Name == trimmedRoleName).Value;

        if (role is null)
        {
            _logger.LogInformation("Role {trimmedRoleName} does not exist. Creating.", trimmedRoleName);
            
            role = await guild.CreateRoleAsync(roleName, null, color ?? RandomColorUtility.GetRandomColor(), hoisted, mentionable);
        }

        await user.GrantRoleAsync(role);

        var userName = user.Username + "#" + user.Discriminator;
        
        _logger.LogInformation("Added user {userName} to the role {trimmedRoleName}.", userName, trimmedRoleName);
    }
}