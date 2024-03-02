using Meadow;
using Meadow.Foundation.Displays;
using MyProject.Core;

namespace MyProject.RasPi
{
    internal class MyProjectApp : App<RaspberryPi>
    {
        private MyProjectPlatform platform;

        public bool SupportDisplay { get; set; } = false;

        private static void Main(string[] args)
        {
            MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            platform = new MyProjectPlatform(Device, SupportDisplay);
            var c = new MainController();
            
            await c.Initialize(platform);
            _ = c.Run();
        }

        public override Task Run()
        {
            if (platform.Display is GtkDisplay gtk)
            {
                gtk.Run();
            }

            return base.Run();
        }
    }
}