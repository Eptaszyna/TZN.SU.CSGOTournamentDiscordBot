using Microsoft.Extensions.Options;

namespace TZN.SU.CSGOTournamentDiscordBot;

/// <summary>
/// Service for getting teams data from CSV file.
/// </summary>
public class TeamsService
{
    private readonly IOptions<BotOptions> _options;
    private readonly IHostEnvironment _hostEnvironment;

    private readonly IEnumerable<Team> _teams;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="options">Options object, injected by DI.</param>
    /// <param name="hostEnvironment">Host environment object, injected by DI.</param>
    public TeamsService(IOptions<BotOptions> options, IHostEnvironment hostEnvironment)
    {
        _options = options;
        _hostEnvironment = hostEnvironment;

        var csv= File.ReadAllText(_hostEnvironment.ContentRootPath + Path.DirectorySeparatorChar +
                                       _options.Value.CsvFileLocation);

        _teams = new List<Team>();

        foreach (var line in csv.Split("\r\n").Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }
            
            var properties = line.Split(",");

            if (properties.Length != 6)
            {
                throw new Exception("Invalid CSV format. Make sure every row has exactly 6 fields.");
            }
            
            var team = new Team()
            {
                Name = properties[0],
                Captain = properties[1],
                Members = properties[2..6]
            };

            _teams = _teams.Append(team);
        }
    }

    /// <summary>
    /// Retrieves team name for given user.
    /// </summary>
    /// <param name="userName">Discord tag of a user (UserName#0000).</param>
    /// <returns>Team name, if found. Null otherwise.</returns>
    public string? GetTeamNameForMember(string userName)
    {
        return GetTeamForMember(userName)?.Name;
    }
    
    /// <summary>
    /// Retrieves team for given user.
    /// </summary>
    /// <param name="userName">Discord tag of a user (UserName#0000).</param>
    /// <returns>Team object, if found. Null otherwise.</returns>
    public Team? GetTeamForMember(string userName)
    {
        return _teams.FirstOrDefault(team => team.Captain == userName || team.Members.Contains(userName));
    }
    
    /// <summary>
    /// Checks if given user is a team captain.
    /// </summary>
    /// <param name="userName">Discord tag of a user (UserName#0000)</param>
    /// <returns>True, if user is a captain of a team. False otherwise.</returns>
    public bool UserIsCaptain(string userName)
    {
        return _teams.Any(team => team.Captain == userName);
    }
}