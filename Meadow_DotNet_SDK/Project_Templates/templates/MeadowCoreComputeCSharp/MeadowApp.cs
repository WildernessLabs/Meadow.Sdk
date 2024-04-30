using Meadow;
using Meadow.Devices;
using System.Threading.Tasks;

namespace MeadowApplication.Template;

public class MeadowApp : App<F7CoreComputeV2>
{
    public override Task Initialize()
    {
        Resolver.Log.Info("Initialize...");

        return Task.CompletedTask;
    }

    public override Task Run()
    {
        Resolver.Log.Info("Run...");

        Resolver.Log.Info("Hello, Meadow Core-Compute!");

        return Task.CompletedTask;
    }
}