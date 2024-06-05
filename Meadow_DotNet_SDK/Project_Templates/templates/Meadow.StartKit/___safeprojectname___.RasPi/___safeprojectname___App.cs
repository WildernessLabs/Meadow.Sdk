using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Pinouts;
using ___safeprojectname___.Core;

namespace ___safeprojectname___.RasPi;

internal class ___safeprojectname___App : App<RaspberryPi>
{
    private ___safeprojectname___Hardware hardware;
    private MainController mainController;

    public bool SupportDisplay { get; set; } = false;

    public override Task Initialize()
    {
        hardware = new ___safeprojectname___Hardware(Device, SupportDisplay);
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