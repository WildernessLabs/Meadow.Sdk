using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using MyProject.Core;

namespace MyProject.F7Feather
{
    public class MyProjectApp : App<F7FeatherV2>
    {
        private MyProjectPlatform platform;

        public override async Task Initialize()
        {
            platform = new MyProjectPlatform(Device);
            var c = new MainController();
            await c.Initialize(platform);
            _ = c.Run();
        }
    }
}