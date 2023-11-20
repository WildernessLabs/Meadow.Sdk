using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Peripherals.Sensors.Moisture;
using Meadow.Peripherals.Speakers;

namespace SampleApp.Hardware
{
    public interface ISampleAppHardware
	{
        IGraphicsDisplay? Display { get; }

        RgbPwmLed? RgbLed { get; }

        ITemperatureSensor? TemperatureSensor { get; }

        IHumiditySensor? HumiditySensor { get; }

        IToneGenerator? Speaker { get; }

        IButton? LeftButton { get; }

        IButton? RightButton { get; }

        IButton? UpButton { get; }

        IButton? DownButton { get; }
    }
}