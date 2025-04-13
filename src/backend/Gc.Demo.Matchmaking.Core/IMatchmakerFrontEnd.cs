using System;

namespace Gc.Demo.Matchmaking.Core;

public interface IMatchmakerFrontEnd
{
    Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request, CancellationToken cancellationToken);
    Task<MatchmakingState> CancelMatchmaking(string UserId, CancellationToken cancellationToken);
    Task<MatchmakingState> GetMatchmakingState(string UserId, CancellationToken cancellationToken);
}

public class MatchmakingRequest
{
    public required string UserId { get; init; }
    public required string Geo { get; init; }
    public required string GameMode { get; init; }
    public required string Map { get; init; }
}

public class MatchmakingState
{
    public required string TicketId { get; set; }
    public MatchmakingStatus State { get; set; }
    public string? Endpoint { get; set; }
}

public enum MatchmakingStatus
{
    Pending,
    Matched,
    GameSessionFound,
    Cancelling,
    Cancelled,
    Error,
}
