using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using MyProject.Core;
using MyProject.Core.Contracts;

namespace MyProject.RasPi
{
    internal class MyProjectPlatform : IStartKitHardware
    {
        private readonly RaspberryPi device;
        private readonly IPixelDisplay? display = null;
        private readonly ITemperatureSensor temperatureSimulator;
        private readonly IOutputController outputController;

        public MyProjectPlatform(RaspberryPi device, bool supportDisplay)
        {
            this.device = device;

            if (supportDisplay)
            { // only if we have a display attached
                display = new GtkDisplay(ColorMode.Format16bppRgb565);
            }
        }

        public IButton? UpButton => throw new NotImplementedException();

        public IButton? DownButton => throw new NotImplementedException();

        public IButton? LeftButton => throw new NotImplementedException();

        public IButton? RightButton => throw new NotImplementedException();

        public ITemperatureSensor? TemperatureSensor => throw new NotImplementedException();

        public IPixelDisplay? Display => display;

        public IOutputController OutputController => throw new NotImplementedException();

        public IBluetoothService? BluetoothService => throw new NotImplementedException();
    }
}