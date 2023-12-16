using Meadow.Pinouts;

namespace Meadow.RasPi;

internal class MeadowApp : App<Linux<RaspberryPi>>
{
    private static void Main(string[] args)
    {
        MeadowOS.Start(args);
    }

    public override Task Initialize()
    {
        return base.Initialize();
    }

    public override Task Run()
    {
        return base.Run();
    }
}
