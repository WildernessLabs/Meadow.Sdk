using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Pinouts;
using ___SafeProjectName___.Core;

namespace ___SafeProjectName___.RPi;

internal class MeadowApp : App<RaspberryPi>
{
    private ___SafeProjectName___Hardware hardware;
    private MainController mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        hardware = new ___SafeProjectName___Hardware(Device, SupportDisplay);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        if (hardware.Display is GtkDisplay gtk)
        {
            _ = mainController.Run();
            gtk.Run();
        }

        return mainController.Run();
    }
}