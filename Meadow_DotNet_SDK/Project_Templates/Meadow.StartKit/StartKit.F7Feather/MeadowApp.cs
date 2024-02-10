using Meadow;
using Meadow.Devices;
using StartKit.Core;

namespace StartKit.F7Feather;

public class MeadowApp : App<F7FeatherV2>
{
    private F7FeatherHardware _platform;

    public override async Task Initialize()
    {
        _platform = new F7FeatherHardware(Device);
        var c = new MainController();
        await c.Initialize(_platform);
        _ = c.Run();
    }
}
