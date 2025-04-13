using System;
using System.Threading;
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
        DoesNotExist,
        Pending,
        GameSessionFound,
        Cancelling
    }

    public class StartMatchmakingCommand
    {
        public string Geo { get; set; }
        public string GameMode { get; set; }
        public string Map { get; set; }
    }
}
