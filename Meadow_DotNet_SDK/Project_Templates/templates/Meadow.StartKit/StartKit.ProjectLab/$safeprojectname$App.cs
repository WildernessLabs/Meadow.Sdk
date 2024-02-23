using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using $safeprojectname$.Core;

namespace $safeprojectname$.ProjectLab
{
    public class $safeprojectname$App : App<F7CoreComputeV2>
    {
        public override async Task Initialize()
        {
            var platform = new $safeprojectname$Platform(Device);
            var c = new MainController();
            await c.Initialize(platform);
        }

        public override Task Run()
        {
            return base.Run();
        }
    }
}