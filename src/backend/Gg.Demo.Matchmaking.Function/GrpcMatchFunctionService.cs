using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OpenMatch;
namespace Gg.Demo.Matchmaking.Function;

public class GrpcMatchFunctionService(QueryService.QueryServiceClient queryClient, ILogger<GrpcMatchFunctionService> logger) : MatchFunction.MatchFunctionBase
{
    public override async Task Run(RunRequest request, IServerStreamWriter<RunResponse> responseStream, ServerCallContext context)
    {
        logger.LogDebug("Generating proposals for function {ProfileName}", request.Profile.Name);
        if (!request.Profile.Extensions.UnpackExtension<Int32Value>("min-players", out var minPlayersValue, request.Profile.Name, logger))
            return;
        if (!request.Profile.Extensions.UnpackExtension<Int32Value>("max-players", out var maxPlayersValue, request.Profile.Name, logger))
            return;

        foreach (var pool in request.Profile.Pools)
        {
            logger.LogDebug("Generating proposals for pool {PoolName}", pool.Name);
            var pooledTickets = queryClient.QueryTickets(new QueryTicketsRequest { Pool = pool }, cancellationToken: context.CancellationToken);
            await pooledTickets.ResponseHeadersAsync;
            var match = CreateMatch(request);

            await foreach (var ticketGroup in pooledTickets.ResponseStream.ReadAllAsync())
            {
                foreach (var ticket in ticketGroup.Tickets)
                {
                    match.Tickets.Add(ticket);
                    if (match.Tickets.Count >= maxPlayersValue.Value)
                    {
                        await SendProposal(responseStream, match);
                        match = CreateMatch(request);
                    }
                }
            }
            if (match.Tickets.Count >= minPlayersValue.Value)
            {
                await SendProposal(responseStream, match);
            }
            logger.LogDebug("Pool {PoolName} finished", pool.Name);
        }
        logger.LogDebug("All pools finished");
    }

    private async Task SendProposal(IServerStreamWriter<RunResponse> responseStream, Match match)
    {
        logger.LogDebug("Sending match {MatchId} to OpenMatch with {TicketCount} tickets", match.MatchId, match.Tickets.Count);
        await responseStream.WriteAsync(new RunResponse { Proposal = match });
    }

    private static Match CreateMatch(RunRequest request)
    {
        return new Match()
        {
            AllocateGameserver = true,
            MatchId = Guid.NewGuid().ToString(),
            MatchProfile = request.Profile.Name,
            MatchFunction = "default",
        };
    }
}