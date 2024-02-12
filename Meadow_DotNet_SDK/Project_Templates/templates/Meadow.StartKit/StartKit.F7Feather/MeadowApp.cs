using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.F7Feather
{

    public class MeadowApp : App<F7FeatherV2>
    {
        private $safeprojectname$Platform _platform;

        public override async Task Initialize()
        {
            _platform = new $safeprojectname$Platform(Device);
            var c = new MainController();
            await c.Initialize(_platform);
            _ = c.Run();
        }
    }
}