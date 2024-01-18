using Meadow;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Leds;
using Meadow.Pinouts;

namespace MeadowApp
{
    public class MeadowApp : App<Linux<RaspberryPi>>
    {
        RgbLed rgbLed;

        public override Task Initialize()
        {
            Resolver.Log.Info("Initialize...");

            rgbLed = new RgbLed(
                Device.Pins.GPIO16,
                Device.Pins.GPIO20,
                Device.Pins.GPIO21);

            return base.Initialize();
        }

        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            Resolver.Log.Info("Hello, Raspberry Pi!");

            return CycleColors(TimeSpan.FromMilliseconds(1000));
        }

        async Task CycleColors(TimeSpan duration)
        {
            while (true)
            {
                Resolver.Log.Info("Going through each color...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    rgbLed.SetColor((RgbLedColors)i);
                    await Task.Delay(500);
                }

                await Task.Delay(1000);

                Resolver.Log.Info("Blinking through each color (on 500ms / off 500ms)...");
                for (int i = 0; i < (int)RgbLedColors.count; i++)
                {
                    await rgbLed.StartBlink((RgbLedColors)i);
                    await Task.Delay(3000);
                    await rgbLed.StopAnimation();
                    rgbLed.IsOn = false;
                }

                await Task.Delay(1000);
            }
        }

        static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }
    }
}