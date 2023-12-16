using Meadow;
using Meadow.Devices;

namespace StartKit.ProjectLab;

public class MeadowApp : App<F7CoreComputeV2>
{
    public override Task Initialize()
    {
        var platform = new StartKitPlatform(Device);

        return base.Initialize();
    }

    public override Task Run()
    {
        return base.Run();
    }
}