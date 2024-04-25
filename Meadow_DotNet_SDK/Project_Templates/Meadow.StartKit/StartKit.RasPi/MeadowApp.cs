using StartKit.Core;

namespace Meadow.RasPi;

internal class MeadowApp : App<RaspberryPi>
{
    private RaspberryPiHardware hardware;
    private MainController mainController;

    public bool SupportDisplay { get; set; } = false;

    private static void Main(string[] args)
    {
        MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        hardware = new RaspberryPiHardware(Device, SupportDisplay);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        return mainController.Run();
    }
}
