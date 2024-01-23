using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using StartKit.Core;

namespace StartKit.F7Feather
{

    public class MeadowApp : App<F7FeatherV2>
    {
        private StartKitPlatform _platform;

        public override async Task Initialize()
        {
            _platform = new StartKitPlatform(Device);
            var c = new MainController();
            await c.Initialize(_platform);
            _ = c.Run();
        }
    }
}