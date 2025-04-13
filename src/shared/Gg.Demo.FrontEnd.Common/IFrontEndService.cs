using MagicOnion;
using MessagePack;
namespace Gg.Demo.FrontEnd.Common
{
    public interface IFrontEndService: IService<IFrontEndService>
    {
        UnaryResult<MatchmakingStateDto> StartMatchmaking(StartMatchmakingCommand cmd);
        UnaryResult<MatchmakingStateDto> CancelMatchmaking();

        UnaryResult<MatchmakingStateDto> GetMatchmakingState();
    }

    [MessagePackObject]
    public class MatchmakingStateDto
    {
        [Key(0)]
        public string? TicketId { get; set; }
        [Key(1)]
        public MatchmakingStatusDto Status { get; set; }
        [Key(2)]
        public string? Endpoint { get; set; }
    }

    public enum MatchmakingStatusDto
    {
        DoesNotExist,
        Pending,
        GameSessionFound,
        Cancelling
    }

    [MessagePackObject]
    public class StartMatchmakingCommand
    {
        [Key(0)]
        public string Geo { get; set; }
        [Key(1)]
        public string GameMode { get; set; }
        [Key(2)]
        public string Map { get; set; }
    }
}
