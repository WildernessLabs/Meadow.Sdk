using Meadow.Foundation.Displays;
using StartKit.Core;

namespace Meadow.RasPi;

internal class MeadowApp : App<RaspberryPi>
{
    private StartKitPlatform<RaspberryPi> platform;

    public bool SupportDisplay { get; set; } = false;

    private static void Main(string[] args)
    {
        MeadowOS.Start(args);
    }

    public override async Task Initialize()
    {
        platform = new StartKitPlatform<RaspberryPi>(Device, SupportDisplay);
        var c = new MainController();
        await c.Initialize(platform);
        _ = c.Run();
    }

    public override Task Run()
    {
        if (platform.GetDisplay() is GtkDisplay gtk)
        {
            gtk.Run();
        }

        return base.Run();
    }
}
