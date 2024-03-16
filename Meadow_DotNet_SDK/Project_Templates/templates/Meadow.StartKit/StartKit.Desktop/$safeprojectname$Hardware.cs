using Meadow;
using Meadow.Foundation.Displays;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Sensors;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Atmospheric;
using Meadow.Peripherals.Sensors.Buttons;
using Meadow.Units;
using $safeprojectname$.Core;
using $safeprojectname$.Core.Contracts;

namespace $safeprojectname$.DT
{
    internal class $safeprojectname$Hardware : I$safeprojectname$Hardware
    {
        private readonly Desktop device;
        private readonly Keyboard keyboard;

        public RotationType DisplayRotation => RotationType.Default;
        public IOutputController OutputController { get; }
        public INetworkController? NetworkController { get; }
        public IPixelDisplay? Display => device.Display;
        public ITemperatureSensor? TemperatureSensor { get; }
        public IButton? RightButton { get; }
        public IButton? LeftButton { get; }

        public $safeprojectname$Hardware(Desktop device)
        {
            this.device = device;

            keyboard = new Keyboard();
            NetworkController = new NetworkController(keyboard);

            TemperatureSensor = new SimulatedTemperatureSensor(
                new Temperature(70, Temperature.UnitType.Fahrenheit),
                keyboard.Pins.Up.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                keyboard.Pins.Down.CreateDigitalInterruptPort(InterruptMode.EdgeRising));

            LeftButton = new PushButton(keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));
            RightButton = new PushButton(keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeFalling));

            OutputController = new OutputController();
        }
    }
}