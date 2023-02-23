﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MeadowApp
{
	// Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7FeatherV2>
	{
		RgbPwmLed onboardLed;

		public override Task Run()
		{
			Resolver.Log.Info("Run...");

			CycleColors(TimeSpan.FromMilliseconds(1000));
			return base.Run();
		}

		public override Task Initialize()
		{
			Resolver.Log.Info("Initialize...");

			onboardLed = new RgbPwmLed(
				redPwmPin: Device.Pins.OnboardLedRed,
				greenPwmPin: Device.Pins.OnboardLedGreen,
				bluePwmPin: Device.Pins.OnboardLedBlue,
				CommonType.CommonAnode);

			return base.Initialize();
		}

		void CycleColors(TimeSpan duration)
		{
			Resolver.Log.Info("Cycle colors...");

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
	}
}