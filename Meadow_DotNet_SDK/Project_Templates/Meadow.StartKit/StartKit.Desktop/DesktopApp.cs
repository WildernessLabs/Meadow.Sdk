using Meadow;
using Meadow.Foundation.Displays;
using StartKit.Core;

namespace StartKit.Windows;

public static class Program
{
    private static void Main(string[] args)
    {
#if WINDOWS
        ApplicationConfiguration.Initialize();
#endif
        MeadowOS.Start(args);
    }
}

internal class MeadowApp : App<Desktop>
{
    private MainController mainController;

    public override Task Initialize()
    {
        var hardware = new DesktopHardware(Device);
        mainController = new MainController();
        return mainController.Initialize(hardware);
    }

    public override Task Run()
    {
        // this must be spawned in a worker because the UI needs the main thread
        _ = mainController.Run();

        ExecutePlatformDisplayRunner();

        return base.Run();
    }
    private void ExecutePlatformDisplayRunner()
    {
#if WINDOWS
        System.Windows.Forms.Application.Run(Device.Display as System.Windows.Forms.Form);
#else
        if (Device.Display is GtkDisplay gtk)
        {
            gtk.Run();
        }
#endif
    }

}