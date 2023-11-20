using Avalonia;
using Avalonia.ReactiveUI;
using Meadow;
using Meadow.Simulation;
using System;
using System.Threading.Tasks;

namespace MeadowApp
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            MeadowOS.Start(args);
            //    Resolver.Services.Add<IMeadowDevice>(new SimulatedMeadow<SimulatedPinout>());

            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }

    public class MeadowApp : App<SimulatedMeadow<SimulatedPinout>>
    {
        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override Task Run()
        {
            return base.Run();
        }
    }
}