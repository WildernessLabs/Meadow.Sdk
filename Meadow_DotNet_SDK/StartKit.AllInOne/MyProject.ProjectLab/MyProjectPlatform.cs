using Meadow.Devices;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using MyProject.Core;
using MyProject.Core.Contracts;

namespace MyProject.ProjectLab
{

    internal class MyProjectPlatform : IStartKitHardware
    {
        private F7CoreComputeV2 device;
        // TODO private IProjectLabHardware projLab;
        private IOutputController? outputController;
        private IBluetoothService? bluetoothService;

        public MyProjectPlatform(F7CoreComputeV2 device)
        {
            this.device = device;
            // TODO projLab = Meadow.Devices.ProjectLab.Create();
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