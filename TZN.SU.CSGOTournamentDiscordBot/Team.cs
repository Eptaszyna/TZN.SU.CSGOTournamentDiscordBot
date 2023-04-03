namespace TZN.SU.CSGOTournamentDiscordBot;

/// <summary>
/// Team model.
/// </summary>
public class Team
{
    /// <summary>
    /// Name of the team.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Discord tag of the captain of the team.
    /// </summary>
    public string Captain { get; set; } = null!;
    
    private IEnumerable<string> _members = null!;

    /// <summary>
    /// List of Discord tags of the members of the team.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown, when passing an IEnumerable with item count other than 4.</exception>
    public IEnumerable<string> Members
    {
        get => _members;
        set
        {
            if (value.Count() != 4)
            {
                throw new ArgumentException("Member count must be 4.", nameof(value));
            }

            _members = value;
        }
    }
}