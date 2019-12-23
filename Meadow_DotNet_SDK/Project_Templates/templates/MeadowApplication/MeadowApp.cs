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
	const int pulseDuration = 3000;
	RgbPwmLed rgbPwmLed;

	public MeadowApp()
	{
		rgbPwmLed = new RgbPwmLed(Device,
				   Device.Pins.OnboardLedRed,
				   Device.Pins.OnboardLedGreen,
				   Device.Pins.OnboardLedBlue);

		PulseRgbPwmLed();
	}

	protected void PulseRgbPwmLed()
	{
		while (true)
		{
			Pulse(Color.Red);
			Pulse(Color.Green);
			Pulse(Color.Blue);
		}
	}

	protected void Pulse(Color color)
	{
		rgbPwmLed.StartPulse(color);
		Console.WriteLine($"Pulsing {color}");
		Thread.Sleep(pulseDuration);
		rgbPwmLed.Stop();
	}
}
}
