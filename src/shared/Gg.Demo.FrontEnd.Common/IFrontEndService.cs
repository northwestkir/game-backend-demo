using MagicOnion;

namespace Gg.Demo.FrontEnd.Common
{
    public interface IFrontEndService: IService<IFrontEndService>
    {
        UnaryResult<MatchmakingStateDto> StartMatchmaking(StartMatchmakingCommand cmd);
        UnaryResult<MatchmakingStateDto> CancelMatchmaking();

        UnaryResult<MatchmakingStateDto> GetMatchmakingState();
    }

    public class MatchmakingStateDto
    {
        public string TicketId { get; set; }
        public MatchmakingStatusDto Status { get; set; }
        
        public string? Endpoint { get; set; }
    }

    public enum MatchmakingStatusDto
    {
        Pending,
        Matched,
        GameSessionFound,
        Cancelling,
        Cancelled,
        Error,
    }

    public class StartMatchmakingCommand
    {

    }
}
