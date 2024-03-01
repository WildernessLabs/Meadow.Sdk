using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.RasPi {

    internal class $safeprojectname$Platform<T> : I$safeprojectname$Platform
        where T : IPinDefinitions, new()
    {
        private readonly Meadow.Linux<T> _device;
        private readonly IPixelDisplay? _graphicsDisplay = null;
        private readonly ITemperatureSensor _temperatureSimulator;
        private readonly IOutputService _outputService;

        public $safeprojectname$Platform(Meadow.Linux<T> device, bool supportDisplay)
        {
            _device = device;

            if (supportDisplay)
            { // only if we have a display attached
                _graphicsDisplay = new GtkDisplay(ColorMode.Format16bppRgb565);
            }
        }

        public IBluetoothService? GetBluetoothService()
        {
            return null;
        }

        public IPixelDisplay? GetDisplay()
        {
            return _graphicsDisplay;
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
            return _temperatureSimulator;
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