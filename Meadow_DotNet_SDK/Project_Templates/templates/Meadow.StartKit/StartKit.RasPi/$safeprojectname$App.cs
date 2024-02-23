using System.Threading.Tasks;
using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Pinouts;
using $safeprojectname$.Core;

namespace $safeprojectname$.RasPi
{
    internal class $safeprojectname$App : App<Linux<RaspberryPi>>
    {
        private $safeprojectname$Platform<RaspberryPi> _platform;

        public bool SupportDisplay { get; set; } = false;

        private static void Main(string[] args)
        {
            MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            _platform = new $safeprojectname$Platform<RaspberryPi>(Device, SupportDisplay);
            var c = new MainController();
            await c.Initialize(_platform);
            _ = c.Run();
        }

        public override Task Run()
        {
            if (_platform.GetDisplay() is GtkDisplay gtk)
            {
                gtk.Run();
            }

            return base.Run();
        }
    }
}