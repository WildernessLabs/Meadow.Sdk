using System;
using System.Threading.Tasks;
using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.F7
{
    internal class $safeprojectname$ProjectLabHardware : I$safeprojectname$Hardware
    {
        private readonly IProjectLabHardware projLab;

        public RotationType DisplayRotation => RotationType._270Degrees;
        public IOutputController OutputController { get; }
        public IButton? LeftButton => projLab.LeftButton;
        public IButton? RightButton => projLab.RightButton;
        public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;
        public IPixelDisplay? Display => projLab.Display;
        public INetworkController NetworkController { get; }

        public $safeprojectname$ProjectLabHardware(F7CoreComputeV2 device)
        {
            projLab = ProjectLab.Create();

            OutputController = new OutputController(projLab.RgbLed);
            NetworkController = new NetworkController(device);
        }
    }
}