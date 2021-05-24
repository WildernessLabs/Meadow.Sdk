Imports System
Imports System.Threading
Imports Meadow
Imports Meadow.Devices
Imports Meadow.Hardware
Imports Meadow.Foundation
Imports Meadow.Foundation.Leds

Public Class MeadowApp

	Inherits App(Of F7Micro, MeadowApp)

	private Dim onboardLed as RgbPwmLed

	Public Sub New()
		MyBase.New

		Console.WriteLine("Hello VB.NET!")

		Initialize()
		CycleColors(1000)

	End Sub

	Private Sub Initialize()
		Console.WriteLine("Initialize hardware...")

		onboardLed = new RgbPwmLed (Device,
			Device.Pins.OnboardLedRed,
			Device.Pins.OnboardLedGreen,
			Device.Pins.OnboardLedBlue,
			3.3f, 3.3f, 3.3f,
			Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode)

	End Sub

	Private Sub CycleColors(ByVal duration As Integer)
		Console.WriteLine("Cycle colors...")

		While true
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

	Private Sub ShowColorPulse(ByVal color As Color, ByVal duration As Integer)
		onboardLed.StartPulse(color, duration/2)
		Thread.Sleep(duration)
	End Sub

End Class
