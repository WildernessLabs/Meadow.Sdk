using Meadow;

namespace MeadowApplication.Template
{
    public class MeadowApp : LinuxApp<RaspberryPi>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Raspberry Pi!");

            return Task.CompletedTask;
        }

        static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}