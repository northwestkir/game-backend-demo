using Gg.Demo.FrontEnd.Common;
using MagicOnion;
using MagicOnion.Server;

namespace Gg.Demo.FrontEnd.Core;

public class MagicOnionFrontEndService : ServiceBase<IFrontEndService>, IFrontEndService
{
    public UnaryResult<MatchmakingState> GetMatchmakingState()
    {
        throw new NotImplementedException();
    }

    public UnaryResult<MatchmakingState> StartMatchmaking(StartMatchmakingCommand cmd)
    {
        throw new NotImplementedException();
    }

    public UnaryResult<MatchmakingState> StopMatchmaking()
    {
        throw new NotImplementedException();
    }
}
