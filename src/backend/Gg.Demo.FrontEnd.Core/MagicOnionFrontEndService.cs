using System.Security.Claims;
using Gg.Demo.Backend.Abstractions;
using Gg.Demo.FrontEnd.Common;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.AspNetCore.Http;

namespace Gg.Demo.FrontEnd.Core;

public class MagicOnionFrontEndService(IClusterClient client, IHttpContextAccessor httpContextAccessor) : ServiceBase<IFrontEndService>, IFrontEndService
{
    public async UnaryResult<MatchmakingStateDto> GetMatchmakingState()
    {
        var cancellationToken = this.Context.CallContext.CancellationToken;
        var userId = GetUserId();
        
        var grain = client.GetGrain<IMatchmakingGrain>(userId);
        using var grainCts = new GrainCancellationTokenSource();
        
        var state = await grain.GetMatchmakingState();
        return new MatchmakingStateDto
        {
            TicketId = state.TicketId,
            Endpoint = state.Endpoint,
            Status = Convert(state.State),
        };
    }

    public async UnaryResult<MatchmakingStateDto> StartMatchmaking(StartMatchmakingCommand cmd)
    {        
        var userId = GetUserId();
        var cancellationToken = Context.CallContext.CancellationToken;
        var grain = client.GetGrain<IMatchmakingGrain>(userId);
        using var grainCts = new GrainCancellationTokenSource();
        var state = await grain.StartMatchmaking(Convert(cmd));
        return new MatchmakingStateDto
        {
            TicketId = state.TicketId,
            Endpoint = state.Endpoint,
            Status = Convert(state.State),
        };
    }

    public async UnaryResult<MatchmakingStateDto> CancelMatchmaking()
    {
        var userId = GetUserId();
        var cancellationToken = Context.CallContext.CancellationToken;
        var grain = client.GetGrain<IMatchmakingGrain>(userId);
        
        var state = await grain.CancelMatchmaking();
        return new MatchmakingStateDto
        {
            TicketId = state.TicketId,
            Endpoint = state.Endpoint,
            Status = Convert(state.State),
        };
    }


    private Guid GetUserId()
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User ID is not available");
        }
        return Guid.Parse(userId);
    }

    private static MatchmakingRequest Convert(StartMatchmakingCommand cmd)
    {
        return new MatchmakingRequest
        {
            Geo = cmd.Geo,
            GameMode = cmd.GameMode,
            Map = cmd.Map,
        };
    }
    private MatchmakingStatusDto Convert(MatchmakingStatus status)
    {
        return status switch
        {
            MatchmakingStatus.Pending => MatchmakingStatusDto.Pending,
            MatchmakingStatus.GameSessionFound => MatchmakingStatusDto.GameSessionFound,
            MatchmakingStatus.Cancelling => MatchmakingStatusDto.Cancelling,
            MatchmakingStatus.DoesNotExist => MatchmakingStatusDto.DoesNotExist,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, $"Invalid matchmaking status: {status}")
        };
    }
}
