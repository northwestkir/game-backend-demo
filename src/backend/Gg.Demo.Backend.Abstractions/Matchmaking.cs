namespace Gg.Demo.Backend.Abstractions;

[Alias(nameof(IMatchmakingGrain))]
public interface IMatchmakingGrain : IGrainWithGuidKey
{
    [Alias(nameof(StartMatchmaking))]
    Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request);
    [Alias(nameof(CancelMatchmaking))]
    Task<MatchmakingState> CancelMatchmaking();
    [Alias(nameof(GetMatchmakingState))]
    Task<MatchmakingState> GetMatchmakingState();
}

[GenerateSerializer, Alias(nameof(MatchmakingRequest))]
public class MatchmakingRequest
{
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
    DoesNotExist,
    Pending,
    GameSessionFound,
    Cancelling
}

