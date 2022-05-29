open System
open Meadow.Devices
open Meadow
open Meadow.Foundation.Leds
open Meadow.Foundation

type MeadowApp() =
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    inherit App<F7FeatherV2, MeadowApp>()
        
    do Console.WriteLine "Initialize hardware... [F#]"
    let led = new RgbPwmLed(MeadowApp.Device, MeadowApp.Device.Pins.OnboardLedRed,MeadowApp.Device.Pins.OnboardLedGreen, MeadowApp.Device.Pins.OnboardLedBlue,Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode)
    
    let ShowColorPulse (color : Color) (duration : TimeSpan)  = 
        led.StartPulse(color, duration.Divide(2)) |> ignore
        Threading.Thread.Sleep (duration) |> ignore
        led.Stop |> ignore
    
    let cyclecolors (duration : TimeSpan) = 
        while true do
            ShowColorPulse Color.Blue duration 
            ShowColorPulse Color.Cyan duration
            ShowColorPulse Color.Green duration
            ShowColorPulse Color.GreenYellow duration
            ShowColorPulse Color.Yellow duration
            ShowColorPulse Color.Orange duration
            ShowColorPulse Color.OrangeRed duration
            ShowColorPulse Color.Red duration
            ShowColorPulse Color.MediumVioletRed duration
            ShowColorPulse Color.Purple duration
            ShowColorPulse Color.Magenta duration
            ShowColorPulse Color.Pink duration
            
    do cyclecolors (TimeSpan.FromSeconds(1))

[<EntryPoint>]
let main argv =
    let app = new MeadowApp()
    Threading.Thread.Sleep (System.Threading.Timeout.Infinite)
    0 // return an integer exit code