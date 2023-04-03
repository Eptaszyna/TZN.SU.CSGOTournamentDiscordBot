namespace TZN.SU.CSGOTournamentDiscordBot;

/// <summary>
/// Options class for setting configuration values.
/// </summary>
public class BotOptions
{
    /// <summary>
    /// Location of the CSV file with teams data.
    /// </summary>
    public string CsvFileLocation { get; set; } = "AppData/data.csv";

    /// <summary>
    /// Token of the bot, taken from Discord Developer Portal.
    /// </summary>
    public string BotToken { get; set; } = null!;
    
    /// <summary>
    /// Name of a role that will be assigned to all team captains.
    /// </summary>
    public string CaptainsRoleName { get; set; } = "Kapitanowie";

    /// <summary>
    /// Name of a role that will be assigned to all players (users who are
    /// either captains or members of a team).
    /// </summary>
    public string PlayersRoleName { get; set; } = "Zawodnicy";
}