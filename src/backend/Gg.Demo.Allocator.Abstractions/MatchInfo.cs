namespace Gg.Demo.Allocator.Abstractions;

public class MatchInfo
{
    public string MatchId { get; set; }
    public string Map { get; set; }
    public string GameMode { get; set; }
    public string Geo { get; set; }

    public IEnumerable<PlayerInfo> Players { get; set; }
}


public class PlayerInfo
{
    public Guid PlayerId { get; set; }
    public string TicketId { get; set; }
}