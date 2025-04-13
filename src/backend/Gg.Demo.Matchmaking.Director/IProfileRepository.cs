using OpenMatch;

namespace Gg.Demo.Matchmaking.Director;

public interface IProfileRepository
{
    IEnumerable<MatchProfile> GetProfiles();
}
