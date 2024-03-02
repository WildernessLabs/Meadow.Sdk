using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using MyProject.Core;
using MyProject.Core.Contracts;

namespace MyProject.F7Feather
{

    internal class MyProjectPlatform : IStartKitHardware
    {
        private readonly F7FeatherBase device;
        private readonly IPixelDisplay? display = null;
        private readonly ITemperatureSensor temperatureSensor;
        private readonly IOutputController outputController;

        public MyProjectPlatform(F7FeatherBase device)
        {
            this.device = device;
            temperatureSensor = new Bme688(device.CreateI2cBus());
            outputController = new OutputController(
                new RgbLed(
                    this.device.Pins.OnboardLedRed.CreateDigitalOutputPort(),
                    this.device.Pins.OnboardLedGreen.CreateDigitalOutputPort(),
                    this.device.Pins.OnboardLedBlue.CreateDigitalOutputPort()
                    )
                );
        }

        public IButton? UpButton => throw new NotImplementedException();

        public IButton? DownButton => throw new NotImplementedException();

        public IButton? LeftButton => throw new NotImplementedException();

        public IButton? RightButton => throw new NotImplementedException();

        public ITemperatureSensor? TemperatureSensor => throw new NotImplementedException();

        public IPixelDisplay? Display => throw new NotImplementedException();

        public IOutputController OutputController => throw new NotImplementedException();

        public IBluetoothService? BluetoothService => throw new NotImplementedException();
    }
}