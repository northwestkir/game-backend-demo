using System.Runtime.CompilerServices;
using Gg.Demo.Allocator.Abstractions;
using Grpc.Core;
using Microsoft.Extensions.Options;
using OpenMatch;

namespace Gg.Demo.Matchmaking.Director;

public class OpenMatchBackendClient(
    BackendService.BackendServiceClient client,
    IOptions<FunctionConfig> functionConfig,
    ILogger<OpenMatchBackendClient> logger)
{
    public async IAsyncEnumerable<MatchInfo> FetchMatches(IEnumerable<MatchProfile> profiles, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        foreach (var profile in profiles)
        {
            await foreach (var match in FetchMatches(profile, cancellationToken))
            {
                yield return match;
            }
        }
    }
    
    private async IAsyncEnumerable<MatchInfo> FetchMatches(MatchProfile profile, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new FetchMatchesRequest
        {
            Profile = profile,
            Config = functionConfig.Value
        };
        var response = client.FetchMatches(request, cancellationToken: cancellationToken);
        var headers = await response.ResponseHeadersAsync;
        logger.LogInformation("FetchMatches response headers: {Headers}", headers);
        await foreach (var match in response.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return Convert(match);
        }
    }

    public async Task AssignTickets(MatchInfo match, GameServer gameServer, CancellationToken cancellationToken)
    {
        logger.LogDebug("Assigning tickets for match: {MatchId}", match.MatchId);
        var assignmentGroup = new AssignmentGroup { Assignment = new Assignment { Connection = gameServer.Endpoint } };
        assignmentGroup.TicketIds.AddRange(match.Players.Select(p => p.TicketId));

        var request = new AssignTicketsRequest { Assignments = { assignmentGroup } };
        var response = await client.AssignTicketsAsync(request, cancellationToken: cancellationToken);

        logger.LogDebug("Tickets assigned for match: {MatchId}", match.MatchId);
        foreach(var failures in response.Failures)
        {
            logger.LogWarning("Failed to assign ticket: {TicketId} for match: {MatchId}", failures.TicketId, match.MatchId);
        }
    }

    private MatchInfo Convert(FetchMatchesResponse match)
    {
        throw new NotImplementedException();
    }
}
