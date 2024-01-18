using Meadow;
using Meadow.Pinouts;

namespace MeadowApp
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Raspberry Pi!");

            return base.Run();
        }

        static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}