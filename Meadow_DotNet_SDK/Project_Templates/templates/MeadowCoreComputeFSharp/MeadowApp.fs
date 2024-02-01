namespace MeadowApplication.Template

open Meadow
open Meadow.Devices
open System.Threading.Tasks

type MeadowApp() =
    inherit App<F7CoreComputeV2>()

    override this.Initialize() =
        do Resolver.Log.Info "Initialize..."

        Task.CompletedTask
        
    override this.Run () =
        do Resolver.Log.Info "Run..."

        do Resolver.Log.Info "Hello, Meadow Core-Compute!"

        Task.CompletedTask