using System;
using System.Threading;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;

namespace MeadowApp
{
	// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
	public class MeadowApp : App<F7FeatherV2, MeadowApp>
	{
		RgbPwmLed onboardLed;

		public MeadowApp()
		{
			Initialize();
			CycleColors(TimeSpan.FromMilliseconds(1000));
		}

		void Initialize()
		{
			Console.WriteLine("Initialize hardware...");

			onboardLed = new RgbPwmLed(device: Device,
				redPwmPin: Device.Pins.OnboardLedRed,
				greenPwmPin: Device.Pins.OnboardLedGreen,
				bluePwmPin: Device.Pins.OnboardLedBlue,
				Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
		}

		void CycleColors(TimeSpan duration)
		{
			Console.WriteLine("Cycle colors...");

			while (true)
			{
				ShowColorPulse(Color.Blue, duration);
				ShowColorPulse(Color.Cyan, duration);
				ShowColorPulse(Color.Green, duration);
				ShowColorPulse(Color.GreenYellow, duration);
				ShowColorPulse(Color.Yellow, duration);
				ShowColorPulse(Color.Orange, duration);
				ShowColorPulse(Color.OrangeRed, duration);
				ShowColorPulse(Color.Red, duration);
				ShowColorPulse(Color.MediumVioletRed, duration);
				ShowColorPulse(Color.Purple, duration);
				ShowColorPulse(Color.Magenta, duration);
				ShowColorPulse(Color.Pink, duration);
			}
		}

		void ShowColorPulse(Color color, TimeSpan duration)
		{
			onboardLed.StartPulse(color, duration / 2);
			Thread.Sleep(duration);
			onboardLed.Stop();
		}

		void ShowColor(Color color, TimeSpan duration)
		{
			Console.WriteLine($"Color: {color}");
			onboardLed.SetColor(color);
			Thread.Sleep(duration);
			onboardLed.Stop();
		}
	}
}