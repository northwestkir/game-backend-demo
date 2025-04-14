using OpenMatch;

namespace Gg.Demo.Matchmaking.Director;

public class ProfileRepository : IProfileRepository
{
    private readonly MatchProfile _pve;
    private readonly MatchProfile _pvp;
    public ProfileRepository()
    {
        _pve = ConfigureProfile("pve", ["sand", "water"], "pve", 1, 4);
        _pvp = ConfigureProfile("pvp", ["sand", "water"], "pvp", 2, 8);
    }
    public IEnumerable<MatchProfile> GetProfiles()
    {
        return [_pve, _pvp];
    }
    
    private static MatchProfile ConfigureProfile(string name, string[] maps, string mode, int minPlayers, int maxPlayers)
    {
        var options = new MatchProfile();
        options.Name = name;
        
        foreach (var map in maps)
        {
            options.Pools.Add(
                    new Pool()
                    {
                        Name = $"map:{map}_mode:{mode}",
                        StringEqualsFilters = {
                        new StringEqualsFilter() { StringArg = "map", Value = map },
                        new StringEqualsFilter() { StringArg = "mode", Value = mode }
                    }
                    });
        }
        
        options.Extensions.PackExtension(WellKnownExtensions.MinPlayers, minPlayers);
        options.Extensions.PackExtension(WellKnownExtensions.MaxPlayers, maxPlayers);
        return options;
    }
}