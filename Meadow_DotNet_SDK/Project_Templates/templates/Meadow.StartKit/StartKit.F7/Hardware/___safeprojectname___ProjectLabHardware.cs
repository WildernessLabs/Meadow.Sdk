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
using ___safeprojectname___.Core;
using ___safeprojectname___.Core.Contracts;

namespace ___safeprojectname___.F7
{
    internal class ___safeprojectname___ProjectLabHardware : I___safeprojectname___Hardware
    {
        private readonly IProjectLabHardware projLab;

        public RotationType DisplayRotation => RotationType._270Degrees;
        public IOutputController OutputController { get; }
        public IButton? LeftButton => projLab.LeftButton;
        public IButton? RightButton => projLab.RightButton;
        public ITemperatureSensor? TemperatureSensor => projLab.TemperatureSensor;
        public IPixelDisplay? Display => projLab.Display;
        public INetworkController NetworkController { get; }

        public ___safeprojectname___ProjectLabHardware(F7CoreComputeV2 device)
        {
            projLab = ProjectLab.Create();

            OutputController = new OutputController(projLab.RgbLed);
            NetworkController = new NetworkController(device);
        }
    }
}