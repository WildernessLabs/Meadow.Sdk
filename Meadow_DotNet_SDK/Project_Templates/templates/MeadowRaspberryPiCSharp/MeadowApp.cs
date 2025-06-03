using Meadow;
using System.Threading.Tasks;

namespace MeadowApplication.Template;

public class MeadowApp : App<RaspberryPi>
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
}