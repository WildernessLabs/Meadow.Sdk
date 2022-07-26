namespace MeadowApp

open System
open Meadow.Devices
open Meadow
open Meadow.Foundation.Leds
open Meadow.Foundation
open Meadow.Peripherals.Leds

type MeadowApp() =
    // Change F7MicroV2 to F7Micro for V1.x boards
    inherit App<F7FeatherV2>()

    let mutable led : RgbPwmLed = 
        null

    let ShowcolorPulse (color : Color) (duration : TimeSpan) = 
        led.StartPulse(color, duration.Divide(2)) |> ignore
        Threading.Thread.Sleep (duration) |> ignore
        led.Stop |> ignore
    
    let cyclecolors (duration : TimeSpan)  = 
        do Console.WriteLine "Cycle colors..."

        while true do
            ShowcolorPulse Color.Blue duration 
            ShowcolorPulse Color.Cyan duration
            ShowcolorPulse Color.Green duration
            ShowcolorPulse Color.GreenYellow duration
            ShowcolorPulse Color.Yellow duration
            ShowcolorPulse Color.Orange duration
            ShowcolorPulse Color.OrangeRed duration
            ShowcolorPulse Color.Red duration
            ShowcolorPulse Color.MediumVioletRed duration
            ShowcolorPulse Color.Purple duration
            ShowcolorPulse Color.Magenta duration
            ShowcolorPulse Color.Pink duration

    override this.Initialize() =
        do Console.WriteLine "Initialize... (F#)"

        led <- new RgbPwmLed(MeadowApp.Device, 
            MeadowApp.Device.Pins.OnboardLedRed,
            MeadowApp.Device.Pins.OnboardLedGreen, 
            MeadowApp.Device.Pins.OnboardLedBlue, 
            CommonType.CommonAnode)

        base.Initialize()
        
    override this.Run () =
        do Console.WriteLine "Run... (F#)"

        do cyclecolors (TimeSpan.FromSeconds(1))

        base.Run()