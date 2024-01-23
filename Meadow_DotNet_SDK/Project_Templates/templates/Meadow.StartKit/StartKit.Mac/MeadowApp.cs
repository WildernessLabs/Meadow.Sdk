using Meadow;
using StartKit.Core;
using System;

namespace StartKit.Mac
{

    internal class MeadowApp : App<Meadow.Mac>
    {
        private StartKitPlatform _platform;

        private static void Main(string[] args)
        {
            MeadowOS.Start(args);
        }

        public override async Task Initialize()
        {
            _platform = new StartKitPlatform(Device);
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