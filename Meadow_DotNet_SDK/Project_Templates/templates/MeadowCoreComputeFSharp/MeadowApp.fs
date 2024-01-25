namespace MeadowApplication.Template

open Meadow.Devices
open Meadow

type MeadowApp() =
    inherit App<F7CoreComputeV2>()

    override this.Initialize() =
        do Resolver.Log.Info "Initialize..."

        base.Initialize()
        
    override this.Run () =
        do Resolver.Log.Info "Run..."

        do Resolver.Log.Info "Hello, Meadow Core-Compute!"

        base.Run()