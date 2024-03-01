using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.ProjectLab {

    internal class $safeprojectname$Platform : I$safeprojectname$Platform
    {
        private F7CoreComputeV2 _device;
        private IProjectLabHardware _projLab;
        private IOutputService? _outputService;
        private IBluetoothService? _bluetoothService;

        public $safeprojectname$Platform(F7CoreComputeV2 device)
        {
            _device = device;
            _projLab = Meadow.Devices.ProjectLab.Create();
        }

        public IButton? GetUpButton()
        {
            return _projLab.UpButton;
        }

        public IButton? GetDownButton()
        {
            return _projLab.DownButton;
        }

        public IButton? GetLeftButton()
        {
            return _projLab.LeftButton;
        }

        public IButton? GetRightButton()
        {
            return _projLab.RightButton;
        }

        public ITemperatureSensor? GetTemperatureSensor()
        {
            return _projLab.EnvironmentalSensor;
        }

        public IHumiditySensor? GetHumiditySensor()
        {
            return _projLab.EnvironmentalSensor;
        }

        public IPixelDisplay? GetDisplay()
        {
            return _projLab.Display;
        }

        public IOutputService GetOutputService()
        {
            return _outputService ??= new OutputService(_projLab);
        }

        public IBluetoothService GetBluetoothService()
        {
            return _bluetoothService ??= new BluetoothService(_device);
        }
    }
}