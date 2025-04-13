namespace Gg.Demo.Backend.Abstractions;

[Alias(nameof(IMatchmakingGrain))]
public interface IMatchmakingGrain : IGrainWithGuidKey
{
    [Alias(nameof(StartMatchmaking))]
    Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request, CancellationToken cancellationToken);
    [Alias(nameof(CancelMatchmaking))]
    Task<MatchmakingState> CancelMatchmaking(string UserId, CancellationToken cancellationToken);
    [Alias(nameof(GetMatchmakingState))]
    Task<MatchmakingState> GetMatchmakingState(string UserId, CancellationToken cancellationToken);
}

[GenerateSerializer, Alias(nameof(MatchmakingRequest))]
public class MatchmakingRequest
{
    [Id(0)]
    public required string UserId { get; init; }
    [Id(1)]
    public required string Geo { get; init; }
    [Id(2)]
    public required string GameMode { get; init; }
    [Id(3)]
    public required string Map { get; init; }
}

[GenerateSerializer, Alias(nameof(MatchmakingState))]
public class MatchmakingState
{
    [Id(0)]
    public required string TicketId { get; init; }
    [Id(1)]
    public required MatchmakingStatus State { get; init; }
    [Id(2)]
    public string? Endpoint { get; init; }
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

