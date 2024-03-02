using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using MyProject.Core;

namespace MyProject.ProjectLab
{
    public class MyProjectApp : App<F7CoreComputeV2>
    {
        public override async Task Initialize()
        {
            var platform = new MyProjectPlatform(Device);
            var c = new MainController();
            await c.Initialize(platform);
        }

        public override Task Run()
        {
            return base.Run();
        }
    }
}