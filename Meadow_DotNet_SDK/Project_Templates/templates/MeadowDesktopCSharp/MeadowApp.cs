using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;

namespace MeadowApplication.Template;

public class MeadowApp : App<Desktop>
{
    public override Task Initialize()
    {
        Resolver.Log.Info($"Initializing {this.GetType().Name}");
        Resolver.Log.Info($" Platform OS is a {Device.PlatformOS.GetType().Name}");
        Resolver.Log.Info($" Platform: {Device.Information.Platform}");
        Resolver.Log.Info($" OS: {Device.Information.OSVersion}");
        Resolver.Log.Info($" Model: {Device.Information.Model}");
        Resolver.Log.Info($" Processor: {Device.Information.ProcessorType}");

        var displayController = new DisplayController(Device.Display!);

        return base.Initialize();
    }

    public override Task Run()
    {
        // NOTE: this will not return until the display is closed
        ExecutePlatformDisplayRunner();

        return Task.CompletedTask;
    }

    private void ExecutePlatformDisplayRunner()
    {
#if (framework == net8.0-windows)
        if (Device.Display is System.Windows.Forms.Form display)
        {
            System.Windows.Forms.Application.Run(display);
        }
#else
        if (Device.Display is GtkDisplay display)
        {
            display.Run();
        }
#endif
    }
}