using Gg.Demo.Backend.Abstractions;

namespace Gg.Demo.Backend.Core;

public class MatchmakingGrain: IMatchmakingGrain    
{
    public Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MatchmakingState> CancelMatchmaking(string UserId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MatchmakingState> GetMatchmakingState(string UserId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}