using DSharpPlus.Entities;

namespace TZN.SU.CSGOTournamentDiscordBot;

/// <summary>
/// Utility class for getting a random color for a role.
/// </summary>
public static class RandomColorUtility
{
    private static readonly IList<DiscordColor> Colors = new List<DiscordColor>
    {
        DiscordColor.Blurple,
        DiscordColor.Grayple,
        DiscordColor.DarkButNotBlack,
        DiscordColor.NotQuiteBlack,
        DiscordColor.Red,
        DiscordColor.DarkRed,
        DiscordColor.Green,
        DiscordColor.Blue,
        DiscordColor.DarkBlue,
        DiscordColor.Yellow,
        DiscordColor.Cyan,
        DiscordColor.Magenta,
        DiscordColor.Teal,
        DiscordColor.Aquamarine,
        DiscordColor.Gold,
        DiscordColor.Goldenrod,
        DiscordColor.Azure,
        DiscordColor.Rose,
        DiscordColor.SpringGreen,
        DiscordColor.Chartreuse,
        DiscordColor.Orange,
        DiscordColor.Purple,
        DiscordColor.Violet,
        DiscordColor.Brown,
        DiscordColor.HotPink,
        DiscordColor.Lilac,
        DiscordColor.CornflowerBlue,
        DiscordColor.MidnightBlue,
        DiscordColor.Wheat,
        DiscordColor.IndianRed,
        DiscordColor.Turquoise,
        DiscordColor.SapGreen,
        DiscordColor.PhthaloBlue,
        DiscordColor.PhthaloGreen,
        DiscordColor.Sienna
    };

    /// <summary>
    /// Gets a random color, excluding white, black and grays.
    /// </summary>
    /// <returns>A color.</returns>
    public static DiscordColor GetRandomColor()
    {
        var random = new Random();
        var index = random.Next(0, Colors.Count - 1);
        
        return Colors[index];
    }
}