using MagicOnion;

namespace Gg.Demo.FrontEnd.Common
{
    public interface IFrontEndService: IService<IFrontEndService>
    {
        UnaryResult<MatchmakingState> StartMatchmaking(StartMatchmakingCommand cmd);
        UnaryResult<MatchmakingState> StopMatchmaking();

        UnaryResult<MatchmakingState> GetMatchmakingState();
    }

    public class MatchmakingState
    {

    }

    public class StartMatchmakingCommand
    {

    }
}
