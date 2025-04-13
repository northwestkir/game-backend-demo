using System;
using System.Diagnostics.CodeAnalysis;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

namespace Gg.Demo.Matchmaking;

public static class ProtoExtensionsHelpers
{
    public static bool UnpackExtension<T>(this MapField<string, Any> extensions, string name, [NotNullWhen(true)] out T? result, string id, ILogger logger) where T : IMessage, new()
    {
        if (!extensions.TryGetValue(name, out var gsInfoRaw))
        {
            result = default;
            logger.LogError("Required extension {Extension} not found in {Id}", name, id);
            return false;
        }
        //todo: Unpack/TryUnpack uses reflection - consider optimizaiton
        if (gsInfoRaw.TryUnpack(out result))
            return result != null;

        result = default;
        logger.LogError("Failed to unpack {Extension} from {Id}", name, id);
        return false;
    }

    public static void PackExtension(this MapField<string, Any> extensions, string name, int value)
    {
        extensions.Add(name, Any.Pack(new Int32Value { Value = value }));
    }
}
