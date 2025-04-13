using Gg.Demo.Backend.Abstractions;

using Microsoft.Extensions.Logging;

namespace Gg.Demo.Backend.Core;

public class MatchmakingGrain(
    OpenMatchFrontendClient client,
    [PersistentState("matchmaking", "matchmakingStore")] IPersistentState<MatchmakingState> matchmakingState,
    ILogger<MatchmakingGrain> logger) :Grain, IMatchmakingGrain
{   

    public async Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request)
    {
        var cancellationToken = CancellationToken.None;
        Guid userId = this.GetPrimaryKey();
        logger.LogDebug("Starting matchmaking for user {UserId}", userId);
        var currentState = matchmakingState.State;
        if (currentState.State != MatchmakingStatus.DoesNotExist)
        {
            //ask: why log debug but not error?
            logger.LogDebug("Matchmaking already in progress for user {UserId}", userId);
            return currentState;
        }
        var result = await client.StartMatchmaking(request, cancellationToken);
        matchmakingState.State = result;
        logger.LogDebug("Matchmaking started for user {UserId}", userId);
        return result;
    }

    public async Task<MatchmakingState> CancelMatchmaking()
    {
        var cancellationToken = CancellationToken.None; 
        Guid userId = this.GetPrimaryKey();
        if(matchmakingState.State.State == MatchmakingStatus.DoesNotExist)
        {
            logger.LogDebug("Matchmaking does not exist for {UserId}", userId);
            return matchmakingState.State;
        }
        var currentState = matchmakingState.State;
        logger.LogDebug("Cancelling matchmaking for user {UserId}", userId);
        var ticketId = currentState.TicketId;
        var result = await client.CancelMatchmaking(ticketId, cancellationToken);
        matchmakingState.State = result;
        logger.LogDebug("Matchmaking cancelled for user {UserId}", userId);
        return result;
    }

    public async Task<MatchmakingState> GetMatchmakingState()
    {
        var cancellationToken = CancellationToken.None;
        Guid userId = this.GetPrimaryKey();
        logger.LogDebug("Getting matchmaking state for {UserId}", userId);    
        var currentState = matchmakingState.State;
        
        if (currentState.State != MatchmakingStatus.DoesNotExist)
        {
            logger.LogDebug("No Matchmaking data for {UserId}", userId);
            return currentState;
        }
        var ticketId = currentState.TicketId;
        var result = await client.GetMatchmakingState(ticketId, cancellationToken);
        matchmakingState.State = result;
        logger.LogDebug("Matchmaking state for {UserId} is {State}", userId, result.State);
        return result;
    }
}