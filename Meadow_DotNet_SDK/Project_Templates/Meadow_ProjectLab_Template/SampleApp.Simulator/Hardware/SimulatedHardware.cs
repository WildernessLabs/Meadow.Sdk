using Meadow;
using Meadow.Foundation;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Relays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Peripherals.Sensors.Moisture;
using Meadow.Peripherals.Speakers;
using SampleApp.Hardware;

namespace SampleApp.Simulator.Hardware
{
    public class SimulatedHardware : ISampleAppHardware
    {
        public ITemperatureSensor? TemperatureSensor { get; protected set; }

        public IHumiditySensor? HumiditySensor { get; protected set; }

        public IToneGenerator? Speaker { get; protected set; } = null;

        public RgbPwmLed? RgbLed => null;

        public IButton? LeftButton => null;

        public IButton? RightButton => null;

        public IButton? UpButton => null;

        public IButton? DownButton => null;

        public IGraphicsDisplay? Display { get; set; }

        public IMoistureSensor? MoistureSensor { get; set; }

        public SimulatedHardware()
        {
            TemperatureSensor = new TemperatureSensorSimulated(new Meadow.Units.Temperature(20), new Meadow.Units.Temperature(-5), new Meadow.Units.Temperature(45));
            HumiditySensor = new HumiditySensorSimulated(new Meadow.Units.RelativeHumidity(50), new Meadow.Units.RelativeHumidity(0), new Meadow.Units.RelativeHumidity(100));

            Resolver.Log.Info($"Simuated Success!");
        }
    }
}