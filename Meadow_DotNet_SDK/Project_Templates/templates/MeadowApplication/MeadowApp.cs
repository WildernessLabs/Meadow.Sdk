using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace MeadowApp
{
	public class MeadowApp : App<F7Micro, MeadowApp>
	{
		RgbPwmLed onboardLed;

		public MeadowApp()
		{
			Initialize();
			CycleColors(400);
		}

		void Initialize()
		{
			Console.WriteLine("Initialize hardware...");

			onboardLed = new RgbPwmLed(device: Device,
				redPwmPin: Device.Pins.OnboardLedRed,
				greenPwmPin: Device.Pins.OnboardLedGreen,
				bluePwmPin: Device.Pins.OnboardLedBlue,
				3.3f, 3.3f, 3.3f,
				Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
		}

		void CycleColors(int duration)
		{
			Console.WriteLine("Cycle colors...");

			while (true)
			{
				ShowColor(Color.Blue, duration);
				ShowColor(Color.Cyan, duration);
				ShowColor(Color.Green, duration);
				ShowColor(Color.GreenYellow, duration);
				ShowColor(Color.Yellow, duration);
				ShowColor(Color.Orange, duration);
				ShowColor(Color.OrangeRed, duration);
				ShowColor(Color.Red, duration);
				ShowColor(Color.MediumVioletRed, duration);
				ShowColor(Color.Purple, duration);
				ShowColor(Color.Magenta, duration);
				ShowColor(Color.Pink, duration);
			}
		}

		void ShowColor(Color color, int duration = 1000)
		{
			Console.WriteLine($"Color: {color}");
			onboardLed.SetColor(color);
			Thread.Sleep(duration);
			onboardLed.Stop();
		}
	}
}