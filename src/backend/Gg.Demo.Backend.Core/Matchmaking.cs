using Gg.Demo.Backend.Abstractions;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using OpenMatch;
using System.Text.Json;

namespace Gg.Demo.Backend.Core;

public class MatchmakingGrain : IMatchmakingGrain
{
    private readonly GrpcChannel _channel;
    private readonly FrontendService.FrontendServiceClient _client;
    private readonly ILogger<MatchmakingGrain> _logger;
    private readonly Dictionary<string, string> _ticketIds = new();

    public MatchmakingGrain(ILogger<MatchmakingGrain> logger)
    {
        _logger = logger;
        _channel = GrpcChannel.ForAddress("http://open-match-frontend:51504");
        _client = new FrontendService.FrontendServiceClient(_channel);
    }

    public async Task<MatchmakingState> StartMatchmaking(MatchmakingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var ticket = new Ticket
            {
                SearchFields = new SearchFields
                {
                    Tags = { request.GameMode, request.Map },
                    StringArgs = { { "geo", request.Geo } }
                }
            };

            var createTicketRequest = new CreateTicketRequest { Ticket = ticket };
            var response = await _client.CreateTicketAsync(createTicketRequest, cancellationToken: cancellationToken);

            _ticketIds[request.UserId] = response.Ticket.Id;

            return new MatchmakingState
            {
                TicketId = response.Ticket.Id,
                State = MatchmakingStatus.Pending
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create matchmaking ticket for user {UserId}", request.UserId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }

    public async Task<MatchmakingState> CancelMatchmaking(string userId, CancellationToken cancellationToken)
    {
        try
        {
            if (!_ticketIds.TryGetValue(userId, out var ticketId))
            {
                return new MatchmakingState
                {
                    TicketId = string.Empty,
                    State = MatchmakingStatus.Cancelled
                };
            }

            var deleteTicketRequest = new DeleteTicketRequest { TicketId = ticketId };
            await _client.DeleteTicketAsync(deleteTicketRequest, cancellationToken: cancellationToken);

            _ticketIds.Remove(userId);

            return new MatchmakingState
            {
                TicketId = ticketId,
                State = MatchmakingStatus.Cancelled
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel matchmaking for user {UserId}", userId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }

    public async Task<MatchmakingState> GetMatchmakingState(string userId, CancellationToken cancellationToken)
    {
        try
        {
            if (!_ticketIds.TryGetValue(userId, out var ticketId))
            {
                return new MatchmakingState
                {
                    TicketId = string.Empty,
                    State = MatchmakingStatus.Error
                };
            }

            var getTicketRequest = new GetTicketRequest { TicketId = ticketId };
            var response = await _client.GetTicketAsync(getTicketRequest, cancellationToken: cancellationToken);

            var state = response.Ticket.Assignment != null
                ? MatchmakingStatus.GameSessionFound
                : MatchmakingStatus.Pending;

            return new MatchmakingState
            {
                TicketId = ticketId,
                State = state,
                Endpoint = response.Ticket.Assignment?.Connection
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get matchmaking state for user {UserId}", userId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }
}