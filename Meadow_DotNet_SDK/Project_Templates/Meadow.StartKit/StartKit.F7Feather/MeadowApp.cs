using Meadow;
using Meadow.Devices;
using StartKit.Core;

namespace StartKit.F7Feather;

public class MeadowApp : App<F7FeatherV2>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new F7FeatherHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}
