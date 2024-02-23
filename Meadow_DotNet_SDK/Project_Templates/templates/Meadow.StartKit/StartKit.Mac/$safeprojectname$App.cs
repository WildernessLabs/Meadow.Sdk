using System.Threading.Tasks;
using Meadow;
using $safeprojectname$.Core;
using System;

namespace $safeprojectname$.Mac
{
    internal class $safeprojectname$App : App<Mac>
    {
        private $safeprojectname$Platform _platform;

        private static void Main(string[] args)
        {
            MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            _platform = new $safeprojectname$Platform(Device);
            var c = new MainController();
            await c.Initialize(_platform);
            _ = c.Run();
        }

        public override Task Run()
        {
            Console.WriteLine("Hello");
            return base.Run();
        }
    }
}