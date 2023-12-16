using Meadow;
using System;

namespace StartKit.Mac;

internal class MeadowApp : App<Meadow.Mac>
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