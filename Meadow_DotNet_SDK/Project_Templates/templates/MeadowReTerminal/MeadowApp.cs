using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Pinouts;

namespace MeadowApplication.Template
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        GtkDisplay? _display;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            _display = new GtkDisplay(320, 240, ColorMode.Format16bppRgb565);
            var displayController = new DisplayController(_display);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, reTerminal!");

            _display.Run();
        }

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}