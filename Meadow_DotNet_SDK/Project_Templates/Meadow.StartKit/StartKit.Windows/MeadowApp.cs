using Meadow;

namespace StartKit.Windows;

internal class MeadowApp : App<Meadow.Windows>
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