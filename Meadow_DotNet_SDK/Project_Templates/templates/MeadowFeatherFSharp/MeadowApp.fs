﻿namespace MeadowApplication.Template

open System
open Meadow
open Meadow.Devices
open Meadow.Foundation.Leds
open Meadow.Peripherals.Leds
open System.Threading.Tasks

type MeadowApp() =
    inherit App<F7FeatherV2>()

    let mutable led : RgbPwmLed = null

    let ShowColorPulse (color : Color) (duration : TimeSpan) = async {
        do! led.StartPulse(color, TimeSpan.FromMilliseconds(500.0)) |> Async.AwaitTask
        do! Async.Sleep duration
        do! led.StopAnimation() |> Async.AwaitTask
    }

    let CycleColors (duration : TimeSpan) = async {
        do Resolver.Log.Info "Cycle colors..."

        while true do
            do! ShowColorPulse Color.Blue duration 
            do! ShowColorPulse Color.Cyan duration
            do! ShowColorPulse Color.Green duration
            do! ShowColorPulse Color.GreenYellow duration
            do! ShowColorPulse Color.Yellow duration
            do! ShowColorPulse Color.Orange duration
            do! ShowColorPulse Color.OrangeRed duration
            do! ShowColorPulse Color.Red duration
            do! ShowColorPulse Color.MediumVioletRed duration
            do! ShowColorPulse Color.Purple duration
            do! ShowColorPulse Color.Magenta duration
            do! ShowColorPulse Color.Pink duration
    }

    override this.Initialize() =
        do Resolver.Log.Info "Initialize..."

        led <- new RgbPwmLed(
            MeadowApp.Device.Pins.OnboardLedRed,
            MeadowApp.Device.Pins.OnboardLedGreen, 
            MeadowApp.Device.Pins.OnboardLedBlue, 
            CommonType.CommonAnode)

        Task.CompletedTask;

    override this.Run () : Task =
        let runAsync = async {
            do Resolver.Log.Info "Run..."
            do! CycleColors(TimeSpan.FromSeconds(1.0))
        }
        Async.StartAsTask(runAsync) :> Task