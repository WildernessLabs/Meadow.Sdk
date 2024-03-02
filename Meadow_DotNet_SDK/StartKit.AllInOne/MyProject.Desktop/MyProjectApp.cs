using Meadow;
using MyProject.Core;

namespace MyProject.Desktop
{
    internal class MyProjectApp : App<Meadow.Desktop>
    {
        private MyProjectPlatform platform;

        private static void Main(string[] args)
        {

	        MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            platform = new MyProjectPlatform(Device);
            var c = new MainController();
            await c.Initialize(platform);
            _ = c.Run();
        }

        public override Task Run()
        {

	        Application.Run(platform.GetDisplay() as Form);

	        return base.Run();
        }
    }
}