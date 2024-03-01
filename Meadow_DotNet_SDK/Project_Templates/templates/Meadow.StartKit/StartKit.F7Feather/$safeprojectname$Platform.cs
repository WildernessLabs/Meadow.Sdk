using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Atmospheric;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.F7Feather
{

    internal class $safeprojectname$Platform : I$safeprojectname$Platform
    {
        private readonly F7FeatherBase _device;
        private readonly IPixelDisplay? _graphicsDisplay = null;
        private readonly ITemperatureSensor _temperatureSensor;
        private readonly IOutputService _outputService;

        public $safeprojectname$Platform(F7FeatherBase device)
        {
            _device = device;
            _temperatureSensor = new Bme688(device.CreateI2cBus());
            _outputService = new OutputService(
                new RgbLed(
                    _device.Pins.OnboardLedRed.CreateDigitalOutputPort(),
                    _device.Pins.OnboardLedGreen.CreateDigitalOutputPort(),
                    _device.Pins.OnboardLedBlue.CreateDigitalOutputPort()
                    )
                );
        }

        public IBluetoothService? GetBluetoothService()
        {
            return null;
        }

        public IPixelDisplay? GetDisplay()
        {
            return null;
        }

        public IHumiditySensor? GetHumiditySensor()
        {
            return null;
        }

        public IOutputService GetOutputService()
        {
            return _outputService;
        }

        public ITemperatureSensor? GetTemperatureSensor()
        {
            return _temperatureSensor;
        }

        public IButton? GetDownButton()
        {
            return null;
        }

        public IButton? GetLeftButton()
        {
            return null;
        }

        public IButton? GetRightButton()
        {
            return null;
        }

        public IButton? GetUpButton()
        {
            return null;
        }
    }
}