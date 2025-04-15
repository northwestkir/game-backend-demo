namespace Gg.Demo.Allocator.Agones;

public class AgonesAllocatorOptions
{
    public string AllocatorEndpoint { get; set; } = "http://agones-allocator.agones-system.svc.cluster.local";
    public string Namespace { get; set; } = "default";
    public string FleetName { get; set; } = "simple-game-server";
} 