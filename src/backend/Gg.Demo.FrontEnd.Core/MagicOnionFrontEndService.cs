using Gg.Demo.FrontEnd.Common;
using MagicOnion;
using MagicOnion.Server;

namespace Gg.Demo.FrontEnd.Core;

public class MagicOnionFrontEndService : ServiceBase<IFrontEndService>, IFrontEndService
{
    public UnaryResult<MatchmakingStateDto> GetMatchmakingState()
    {
        throw new NotImplementedException();
    }

    public UnaryResult<MatchmakingStateDto> StartMatchmaking(StartMatchmakingCommand cmd)
    {
        throw new NotImplementedException();
    }

    public UnaryResult<MatchmakingStateDto> CancelMatchmaking()
    {
        throw new NotImplementedException();
    }
}
