using Meadow;
using Meadow.Devices;
using StartKit.Core;

namespace StartKit.ProjectLab;

public class MeadowApp : App<F7CoreComputeV2>
{
    public override async Task Initialize()
    {
        var platform = new StartKitPlatform(Device);
        var c = new MainController();
        await c.Initialize(platform);
    }

    public override Task Run()
    {
        return base.Run();
    }
}