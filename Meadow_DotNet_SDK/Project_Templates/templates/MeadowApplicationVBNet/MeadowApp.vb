Imports System
Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Hardware
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds

Public Class MeadowApp
	'Change F7FeatherV2 to F7FeatherV1 for V1.x boards'
	Inherits App(Of F7FeatherV2, MeadowApp)

	Private onboardLed As RgbPwmLed

	Public Sub New()
		MyBase.New

		Initialize()
		CycleColors(TimeSpan.FromMilliseconds(1000))

	End Sub

	Private Sub Initialize()
		Console.WriteLine("Initialize hardware... [VB.NET]")

		onboardLed = New RgbPwmLed(Device,
			Device.Pins.OnboardLedRed,
			Device.Pins.OnboardLedGreen,
			Device.Pins.OnboardLedBlue,
			Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode)

	End Sub

	Private Sub CycleColors(ByVal duration As TimeSpan)
		Console.WriteLine("Cycle colors...")

		While True
			ShowColorPulse(Color.Blue, duration)
			ShowColorPulse(Color.Cyan, duration)
			ShowColorPulse(Color.Green, duration)
			ShowColorPulse(Color.GreenYellow, duration)
			ShowColorPulse(Color.Yellow, duration)
			ShowColorPulse(Color.Orange, duration)
			ShowColorPulse(Color.OrangeRed, duration)
			ShowColorPulse(Color.Red, duration)
			ShowColorPulse(Color.MediumVioletRed, duration)
			ShowColorPulse(Color.Purple, duration)
			ShowColorPulse(Color.Magenta, duration)
			ShowColorPulse(Color.Pink, duration)
		End While

	End Sub

	Private Sub ShowColorPulse(ByVal color As Color, ByVal duration As TimeSpan)
		onboardLed.StartPulse(color, duration / 2)
		Thread.Sleep(duration)
	End Sub

End Class