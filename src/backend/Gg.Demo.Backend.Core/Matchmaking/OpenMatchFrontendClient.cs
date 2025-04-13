using Gg.Demo.Backend.Abstractions;
using Microsoft.Extensions.Logging;
using OpenMatch;

namespace Gg.Demo.Backend.Core;

public class OpenMatchFrontendClient(FrontendService.FrontendServiceClient client, ILogger<OpenMatchFrontendClient> logger)
{
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
            var response = await client.CreateTicketAsync(createTicketRequest, cancellationToken: cancellationToken);
            
            return new MatchmakingState
            {
                TicketId = response.Id,
                State = MatchmakingStatus.Pending,                
                Endpoint = response.Assignment?.Connection, //ask: why assigning here?
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create matchmaking ticket for user {UserId}", request.UserId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }
    public async Task<MatchmakingState> CancelMatchmaking(string ticketId, CancellationToken cancellationToken)
    {
        try
        {

            var deleteTicketRequest = new DeleteTicketRequest { TicketId = ticketId };
            await client.DeleteTicketAsync(deleteTicketRequest, cancellationToken: cancellationToken);


            return new MatchmakingState
            {
                TicketId = ticketId,
                State = MatchmakingStatus.DoesNotExist
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to cancel matchmaking for ticket {TicketId}", ticketId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }

    public async Task<MatchmakingState> GetMatchmakingState(string ticketId, CancellationToken cancellationToken)
    {
        try
        {
            var getTicketRequest = new GetTicketRequest { TicketId = ticketId };
            var response = await client.GetTicketAsync(getTicketRequest, cancellationToken: cancellationToken);

            if(response == null)
            {
                return new MatchmakingState
                {
                    TicketId = ticketId,
                    State = MatchmakingStatus.DoesNotExist
                };
            }
            var state = response.Assignment != null
                ? MatchmakingStatus.GameSessionFound
                : MatchmakingStatus.Pending;

            return new MatchmakingState
            {
                TicketId = ticketId,
                State = state,
                Endpoint = response.Assignment?.Connection
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get matchmaking state for ticket {TicketId}", ticketId);
            return new MatchmakingState
            {
                TicketId = string.Empty,
                State = MatchmakingStatus.Error
            };
        }
    }
    
}
