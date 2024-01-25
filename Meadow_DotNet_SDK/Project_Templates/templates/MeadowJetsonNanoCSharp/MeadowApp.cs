using Meadow;
using Meadow.Pinouts;
using System.Threading.Tasks;

namespace MeadowApplication.Template
{
    public class MeadowApp : App<Linux<JetsonXavierAGX>>
    {
        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Jetson Nano!");

            return base.Run();
        }

        static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}